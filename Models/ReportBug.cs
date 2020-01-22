using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class ReportBug
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        public string User { get; set; }
    }
}
