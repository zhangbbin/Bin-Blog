using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bin_Blog.Web.Data
{
    [Table("tb_blog_post")]
    public class BlogPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Summary { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public List<string> Tags { get; set; } = new();

        [Required]
        public string Content { get; set; } = string.Empty; // 文章正文

        // ====== 与权限 / 用户相关的字段 ======

        /// <summary>
        /// 作者用户 Id（User 表的主键）
        /// </summary>
        [Required]
        public int AuthorId { get; set; }

        /// <summary>
        /// 导航属性：作者实体
        /// </summary>
        public User? Author { get; set; }

        /// <summary>
        /// 是否已发布（false 表示草稿，仅作者本人和管理员可见）
        /// </summary>
        public bool IsPublished { get; set; } = true;

        /// <summary>
        /// 是否软删除（逻辑删除，用于回收站）
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 是否置顶（用于首页 / 列表排序）
        /// </summary>
        public bool IsPinned { get; set; } = false;

        /// <summary>
        /// 是否允许评论（方便针对单篇文章关闭评论）
        /// </summary>
        public bool AllowComments { get; set; } = true;

        // ====== 和交互相关的简单统计字段（点赞/收藏/浏览量等）======

        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int FavoriteCount { get; set; } = 0;

        public int? CategoryId { get; set; } //以此支持未分类文章
        public Category? Category { get; set; }

        // ====== 计算属性示例：阅读时长 ======
        public string ReadingTime
        {
            get
            {
                if (string.IsNullOrEmpty(Content)) return "1 min read";
                // 假设平均阅读速度：中文400字/分钟，英文200词/分钟
                // 这里做一个简单的估算
                var minutes = Content.Length / 400;
                return minutes < 1 ? "1 min read" : $"{minutes} min read";
            }
        }
    }
}
