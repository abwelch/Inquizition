using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class CreateSetup
    {
        [Required]
        public string InquizitorName { get; set; }

        [Required]
        public string SelectedAssessment { get; set; }

        [Required]
        public bool IsPrivate { get; set; }
    }

    public class ColorTheme
    {
        public int ID { get; set; }

        public string Color { get; set; }

        public string InquizitorName { get; set; }
    }
}
