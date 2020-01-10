using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Inquizition.Models
{
    public class UserInfo
    {
        public int ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public bool IntroCompleted { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; }
        [Required]
        public int TotalFriends { get; set; }
        [Required]
        public int TotalSets { get; set; }
        [Required]
        public int TotalBookmarks { get; set; }

        // Add foreign keys for study set tables 

    }
}
