using System.ComponentModel.DataAnnotations;
using Inquizition.Data;
using System.Linq;

namespace Inquizition.Models
{
    public interface IUserInfoManager
    {
        public UserInfo RetrieveUserInfo(string name);
    }

    public class UserInfoManager : IUserInfoManager
    {
        private readonly InquizitionContext _dbContext;

        public UserInfoManager(InquizitionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserInfo RetrieveUserInfo(string name) =>
            _dbContext.UserOverviewInfo.FirstOrDefault(u => u.Username == name);
    }

    // Model for database
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
