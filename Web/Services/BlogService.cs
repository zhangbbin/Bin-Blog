using Bin_Blog.Web.Data;
using Bin_Blog.Web.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;

namespace Bin_Blog.Web.Services
{
    /// <summary>
    /// 博客数据访问层：从数据库(tb_blog_post)中读取文章，而不是使用固定内存数据。
    /// </summary>
    public class BlogService
    {
        private readonly IDbContextFactory<BlogDbContext> _contextFactory;
        private readonly IWebHostEnvironment _environment;

        public BlogService(IDbContextFactory<BlogDbContext> contextFactory, IWebHostEnvironment environment)
        {
            _contextFactory = contextFactory;
            _environment = environment;
        }

        public async Task<int?> CreatePostAsync(string authorUserName, string title, string content, int? categoryId = null, string? summary = null, bool isPublished = true)
        {
            using var context = await _contextFactory.CreateDbContextAsync();

            var author = await context.Users.FirstOrDefaultAsync(u => u.UserName == authorUserName && u.IsActive);
            if (author == null) return null;

            var post = new BlogPost
            {
                Title = title,
                Content = content,
                Summary = summary ?? string.Empty,
                PublishDate = DateTime.UtcNow,
                AuthorId = author.Id,
                IsPublished = isPublished,
                IsDeleted = false,
                CategoryId = categoryId
            };

            context.BlogPosts.Add(post);
            await context.SaveChangesAsync();
            return post.Id;
        }

        /// <summary>
        /// 获取所有已发布、未删除的文章，按发布时间倒序。
        /// </summary>
        public async Task<List<BlogPost>> GetPostsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.BlogPosts
                .Where(p => p.IsPublished && !p.IsDeleted)
                .OrderByDescending(p => p.PublishDate)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键 Id 获取单篇文章。
        /// </summary>
        public async Task<BlogPost?> GetPostByIdAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return context.BlogPosts
                      .AsNoTracking()
                      .FirstOrDefault(p => p.Id == id && !p.IsDeleted);
        }

        // 获取用户信息
        public async Task<User?> GetUserByNameAsync(string username)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, string? newNickName, string newEmail, string? newAvatarUrl, string? newBio)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var user = await context.Users.FindAsync(userId);
            if (user == null) return false;

            user.NickName = newNickName ?? string.Empty;
            user.Email = newEmail;
            user.AvatarUrl = newAvatarUrl;
            user.Bio = newBio;

            await context.SaveChangesAsync();
            return true;
        }

        // ↓↓↓↓↓ 新增：上传头像的方法 ↓↓↓↓↓
        public async Task<string?> UploadAvatarAsync(IBrowserFile file)
        {
            if (file == null) return null;

            try
            {
                // 1. 确保上传目录存在 (wwwroot/images/avatars)
                var uploadPath = Path.Combine(_environment.WebRootPath, "images", "avatars");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // 2. 生成唯一文件名 (防止重名)
                var fileExtension = Path.GetExtension(file.Name);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, fileName);

                // 3. 限制文件大小 (例如最大 5MB)
                var maxFileSize = 1024 * 1024 * 5;

                // 4. 保存文件
                await using var stream = file.OpenReadStream(maxFileSize);
                await using var fs = new FileStream(filePath, FileMode.Create);
                await stream.CopyToAsync(fs);

                // 5. 返回相对 URL
                return $"/images/avatars/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload failed: {ex.Message}");
                return null;
            }
        }

        // 获取所有分类
        public async Task<List<Category>> GetCategoriesAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Categories.AsNoTracking().ToListAsync();
        }

        // 获取文章列表（支持筛选和搜索）
        public async Task<List<BlogPost>> GetPostsAsync(int? categoryId = null, string? keyword = null)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var query = context.BlogPosts.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
            { 
                query = query.Where(p => p.Title.Contains(keyword) || p.Content.Contains(keyword));
            }

            return await query.OrderByDescending(p => p.PublishDate)
                              .AsNoTracking()
                              .ToListAsync();
        }

        // 3. 种子数据初始化（可选，方便你测试）
        public async Task SeedCategoriesAsync()
        {

            using var context = await _contextFactory.CreateDbContextAsync();
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "全部" }, // 逻辑上的全部，ID通常处理
                    new Category { Name = "前端" },
                    new Category { Name = "后端" },
                    new Category { Name = "C++" },
                    new Category { Name = "C#" },
                    new Category { Name = "Java" }
                );
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Announcement>> GetAnnouncementsAsync(int take = 5)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.Announcements
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.IsPinned)
                .ThenByDescending(a => a.PublishDate)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SeedAnnouncementsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            if (!context.Announcements.Any())
            {
                context.Announcements.AddRange(
                    new Announcement
                    {
                        Title = "欢迎来到 Bin-Blog 🎉",
                        Content = "近期正在优化页面样式与文章结构，如有建议欢迎留言。",
                        IsPinned = true,
                        IsPublished = true,
                        PublishDate = DateTime.UtcNow
                    },
                    new Announcement
                    {
                        Title = "内容迁移中",
                        Content = "文章会陆续从旧站迁移过来。",
                        IsPinned = false,
                        IsPublished = true,
                        PublishDate = DateTime.UtcNow.AddDays(-1)
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
