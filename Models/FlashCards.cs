using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Inquizition.Data;

namespace Inquizition.Models
{
    public class FlashCards
    {
        private List<FlashCardEntry> Assessment;
        private int CardCounter { get; set; }
        private readonly InquizitionContext _dbContext;

        public FlashCards(InquizitionContext dbContext)
        {
            _dbContext = dbContext;
            Assessment = new List<FlashCardEntry>();
            CardCounter = 0;
        }

        public bool AddFlashCard()
        {
       
            return true;
        }

        public int CommitToDatabase()
        {

            return 0;
        }
    }

    public class FlashCardEntry
    {
        public int ID { get; set; }

        public string Creator { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 3)]
        public string InquizitorName { get; set; }

        public int CardNumber { get; set; }

        [StringLength(45)]
        public string CardTitle { get; set; }

        [StringLength(300)]
        public string CardBody { get; set; }

        [Required]
        [StringLength(400)]
        public string CardAnswer { get; set; }

        public string Image { get; set; }

        public string Audio { get; set; }
    }
}
