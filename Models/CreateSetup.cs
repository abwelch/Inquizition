using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class CreateSetup
    {
        [Required]
        public string AssessmentName { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 3)]
        public string SelectedAssessment { get; set; }

        [Required]
        public byte IsPrivate { get; set; }
    }
}
