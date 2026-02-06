using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bin_Blog.Web.Data
{
    /// <summary>
    /// 用户角色：用来控制谁可以发文、谁只能评论/点赞等
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// 超级管理员（你自己）：拥有所有权限，包括管理用户、发文等
        /// </summary>
        Admin = 1,

        /// <summary>
        /// 作者：可以发文、编辑自己文章
        /// </summary>
        Author = 2,

        /// <summary>
        /// 普通用户：只能评论、点赞、收藏、关注等
        /// </summary>
        Reader = 3
    }

    [Table("tb_user")]
    /// <summary>
    /// 博客用户实体，后续会通过 EF Core 生成 User 表，并结合 JWT 做登录认证
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 登录用户名（唯一）
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string NickName { get; set; } = string.Empty;

        /// <summary>
        /// 邮箱（可选，但推荐唯一）
        /// </summary>
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 密码哈希（不会明文存储密码）
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// 密码加盐值（用于增强安全性）
        /// </summary>
        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        // 可选：添加个简介字段
        public string? Bio { get; set; }

        /// <summary>
        /// 用户角色：决定能否发文章等
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Reader;

        /// <summary>
        /// 是否启用（封禁用户时可置为 false）
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// 导航属性：该用户发布的文章集合
        /// </summary>
        public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
    }
}
