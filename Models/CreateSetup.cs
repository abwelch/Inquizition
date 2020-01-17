using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class CreateSetup 
    {
        [Required]
        [StringLength(60, ErrorMessage = "Name must be at least 3 characters and no more than 60 characters.", MinimumLength =3)]
        public string InquizitorName { get; set; }

        [Required]
        public string SelectedAssessment { get; set; }

        [Required]
        public bool IsPrivate { get; set; }
    }
}
