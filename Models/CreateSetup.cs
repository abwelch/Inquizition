using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class CreateSetup
    {
        [Required]
        public string InquizitorName { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 3)]
        public string SelectedAssessment { get; set; }

        [Required]
        public bool IsPrivate { get; set; }
    }
}
