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
        private readonly InquizitionContext _dbContext;

        public FlashCards(InquizitionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddFlashCard()
        {
       
            return true;
        }

        public bool InquizitorNameAvailable(string inputtedName) =>
            _dbContext.FlashCards.FirstOrDefault(f => f.InquizitorName == inputtedName) == null ? true : false;

        public int CommitToDatabase()
        {

            return 0;
        }

        public List<FlashCardEntry> RetrieveAllEntries(string inquizName)
        {
            List<FlashCardEntry> Inquizitor = new List<FlashCardEntry>();
            // Retrieve all entries from db matching inquizName and store in list


            return Inquizitor;
        }
    }

    public class FlashCardEntry
    {
        public int ID { get; set; }

        public string Creator { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 3)]
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
