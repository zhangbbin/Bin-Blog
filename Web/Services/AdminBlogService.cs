using Bin_Blog.Web.Data;
using Bin_Blog.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Bin_Blog.Web.Services;

public class AdminBlogService
{
    private readonly IDbContextFactory<BlogDbContext> _contextFactory;

    public AdminBlogService(IDbContextFactory<BlogDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <summary>
    /// 获取文章列表，支持按状态筛选（已发布 / 草稿 / 已删除）
    /// </summary>
    public async Task<List<BlogPost>> GetPostsAsync(PostStatusFilter filter = PostStatusFilter.Published, string? keyword = null)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var query = context.BlogPosts
            .Include(p => p.Author)
            .Include(p => p.Category)
            .AsQueryable();

        query = filter switch
        {
            PostStatusFilter.Published => query.Where(p => p.IsPublished && !p.IsDeleted),
            PostStatusFilter.Draft => query.Where(p => !p.IsPublished && !p.IsDeleted),
            PostStatusFilter.Deleted => query.Where(p => p.IsDeleted),
            _ => query.Where(p => !p.IsDeleted)
        };

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p => p.Title.Contains(keyword));
        }

        return await query
            .OrderByDescending(p => p.IsPinned)
            .ThenByDescending(p => p.PublishDate)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// 获取单篇文章（包含已删除的，管理员可见）
    /// </summary>
    public async Task<BlogPost?> GetPostByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.BlogPosts
            .Include(p => p.Author)
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// 创建文章
    /// </summary>
    public async Task<int?> CreatePostAsync(int authorId, string title, string content, string? summary, int? categoryId, string? tagsRaw, bool isPublished)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        var post = new BlogPost
        {
            AuthorId = authorId,
            Title = title.Trim(),
            Content = content,
            Summary = summary?.Trim() ?? string.Empty,
            CategoryId = categoryId,
            Tags = ParseTags(tagsRaw),
            IsPublished = isPublished,
            IsDeleted = false,
            PublishDate = DateTime.UtcNow
        };

        context.BlogPosts.Add(post);
        await context.SaveChangesAsync();
        return post.Id;
    }

    /// <summary>
    /// 更新文章
    /// </summary>
    public async Task<bool> UpdatePostAsync(int postId, string title, string content, string? summary, int? categoryId, string? tagsRaw, bool isPublished, bool isPinned, bool allowComments)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var post = await context.BlogPosts.FindAsync(postId);
        if (post == null) return false;

        post.Title = title.Trim();
        post.Content = content;
        post.Summary = summary?.Trim() ?? string.Empty;
        post.CategoryId = categoryId;
        post.Tags = ParseTags(tagsRaw);
        post.IsPublished = isPublished;
        post.IsPinned = isPinned;
        post.AllowComments = allowComments;

        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 软删除文章
    /// </summary>
    public async Task<bool> DeletePostAsync(int postId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var post = await context.BlogPosts.FindAsync(postId);
        if (post == null) return false;

        post.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 恢复已删除的文章
    /// </summary>
    public async Task<bool> RestorePostAsync(int postId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var post = await context.BlogPosts.FindAsync(postId);
        if (post == null) return false;

        post.IsDeleted = false;
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// 获取文章统计
    /// </summary>
    public async Task<AdminStats> GetStatsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return new AdminStats
        {
            PublishedCount = await context.BlogPosts.CountAsync(p => p.IsPublished && !p.IsDeleted),
            DraftCount = await context.BlogPosts.CountAsync(p => !p.IsPublished && !p.IsDeleted),
            DeletedCount = await context.BlogPosts.CountAsync(p => p.IsDeleted),
            TotalViews = await context.BlogPosts.Where(p => !p.IsDeleted).SumAsync(p => p.ViewCount)
        };
    }

    private static List<string> ParseTags(string? tagsRaw)
    {
        if (string.IsNullOrWhiteSpace(tagsRaw))
            return [];

        return tagsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                      .Distinct()
                      .ToList();
    }
}

public enum PostStatusFilter
{
    Published,
    Draft,
    Deleted
}

public class AdminStats
{
    public int PublishedCount { get; set; }
    public int DraftCount { get; set; }
    public int DeletedCount { get; set; }
    public int TotalViews { get; set; }
}
