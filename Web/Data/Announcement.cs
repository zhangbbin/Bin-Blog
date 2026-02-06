using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bin_Blog.Web.Data
{
    [Table("tb_announcement")]
    public class Announcement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(4000)]
        public string? Content { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = true;

        public bool IsPinned { get; set; } = false;
    }
}
