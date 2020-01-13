using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class UserInfo
    {
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; }
        [Required]
        public bool IntroCompleted { get; set; }
        public int TotalFriends { get; set; }
        public int TotalSets { get; set; }
        public int TotalBookmarks { get; set; }
        public int ReportedInstances { get; set; }
        public bool Banned { get; set; }

        // Add foreign keys for study set tables 

    }

    // Utilized to store records of users reporting other users
    public class UserReports
    {
        public int ID { get; set; }

        public string Offender { get; set; }

        public string Accuser { get; set; }

        public string ReportedInquizitor { get; set; }

        // Store the comment ID
        public int ReportedComment { get; set; }

    }
}
