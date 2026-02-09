using System.Text;
using Bin_Blog.Components;
using Bin_Blog.Web.Models;
using Bin_Blog.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Configure the database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<BlogDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure services for Razor Components
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<AdminBlogService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddCascadingAuthenticationState();

// Configure JWT authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key is not configured.");
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSection.GetValue<string>("Issuer"),
            ValidAudience = jwtSection.GetValue<string>("Audience"),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CanWrite", policy => policy.RequireRole("Admin", "Author"));
});

// 注册 HttpClient
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<BlogDbContext>>();
    await using var db = await dbFactory.CreateDbContextAsync();
    await db.Database.MigrateAsync();

    var blogService = scope.ServiceProvider.GetRequiredService<BlogService>();
    await blogService.SeedCategoriesAsync();
    await blogService.SeedAnnouncementsAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Minimal API endpoints for auth
var authGroup = app.MapGroup("/api/auth");

authGroup.MapPost("/register", async ([FromBody] RegisterRequest request, AuthService authService) =>
{
    Console.WriteLine("进入注册端点...\n" +
        $"UserName:'{request?.UserName ?? "<null>"}', Email:'{request?.Email ?? "<null>"}', Password:'{(request?.Password is null ? "<null>" : (request.Password.Length > 0 ? "<hidden>" : "<empty>"))}'");

    if (request == null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest("用户名和密码不能为空。");
    }

    var user = await authService.RegisterAsync(request.UserName, request.Email, request.Password);
    if (user == null)
    {
        return Results.Conflict("用户名已存在。");
    }

    return Results.Ok(new { user.Id, user.UserName, Role = user.Role.ToString() });
}).AllowAnonymous();

authGroup.MapPost("/login", async (LoginRequest request, AuthService authService) =>
{
    var user = await authService.ValidateUserAsync(request.UserName, request.Password);
    if (user == null)
    {
        return Results.Unauthorized();
    }

    var token = authService.GenerateJwtToken(user);
    var response = new LoginResponse
    {
        Token = token,
        UserName = user.UserName,
        NickName = string.IsNullOrWhiteSpace(user.NickName) ? user.UserName : user.NickName,
        Role = user.Role.ToString()
    };

    return Results.Ok(response);
}).AllowAnonymous();

authGroup.MapPost("/refresh", async (HttpContext httpContext, AuthService authService) =>
{
    var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();
    if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
    {
        return Results.Unauthorized();
    }

    var token = authHeader["Bearer ".Length..];
    var newToken = await authService.RefreshTokenAsync(token);
    if (newToken == null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new { Token = newToken });
}).RequireAuthorization();

app.Run();
