using Bin_Blog.Web.Data;
using Bin_Blog.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Bin_Blog.Web.Services
{
    /// <summary>
    /// 博客数据访问层：从数据库(tb_blog_post)中读取文章，而不是使用固定内存数据。
    /// </summary>
    public class BlogService
    {
        private readonly BlogDbContext _db;

        public BlogService(BlogDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 获取所有已发布、未删除的文章，按发布时间倒序。
        /// </summary>
        public List<BlogPost> GetPosts()
        {
            return _db.BlogPosts
                      .Where(p => p.IsPublished && !p.IsDeleted)
                      .OrderByDescending(p => p.PublishDate)
                      .AsNoTracking()
                      .ToList();
        }

        /// <summary>
        /// 根据主键 Id 获取单篇文章。
        /// </summary>
        public BlogPost? GetPostById(int id)
        {
            return _db.BlogPosts
                      .AsNoTracking()
                      .FirstOrDefault(p => p.Id == id && !p.IsDeleted);
        }
    }
}
