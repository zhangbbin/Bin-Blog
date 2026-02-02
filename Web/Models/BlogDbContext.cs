using Bin_Blog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Bin_Blog.Web.Models
{
    /// <summary>
    /// 博客系统的 EF Core 上下文。
    /// 后续你可以在服务端项目中使用同样的上下文配置数据库连接、迁移等。
    /// </summary>
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// 博客文章表
        /// </summary>
        public DbSet<BlogPost> BlogPosts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User 配置
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.UserName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(u => u.UserName)
                      .IsUnique();

                entity.Property(u => u.Email)
                      .HasMaxLength(100);
            });

            // BlogPost 配置
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                // 一对多：User(Author) - BlogPosts
                entity.HasOne(p => p.Author)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(p => p.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
