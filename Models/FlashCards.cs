using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Inquizition.Data;

namespace Inquizition.Models
{
    public interface IFlashCards
    {
        public List<FlashCardEntry> Inquizitor { get; set; }

        public FlashCardEntry ExampleCard { get; set; }

        public bool ColorSet { get; set; }

        public bool InquizitorNameAvailable(string input);

        public string CardContainsProfanity(FlashCardEntry card);

        public bool AddFlashCard(FlashCardEntry card);

    }

    public class FlashCards : IFlashCards
    {
        private readonly InquizitionContext _dbContext;

        public List<FlashCardEntry> Inquizitor { get; set; }

        public FlashCardEntry ExampleCard { get; set; }

        public bool ColorSet { get; set; }

        public FlashCards(InquizitionContext dbContext)
        {
            _dbContext = dbContext;
            Inquizitor = new List<FlashCardEntry>();
        }

        public bool AddFlashCard(FlashCardEntry newCard)
        {
        
            return true;
        }

        public bool InquizitorNameAvailable(string inputtedName) =>
            _dbContext.FlashCards.FirstOrDefault(f => f.InquizitorName == inputtedName) == null ? true : false;

        public string CardContainsProfanity(FlashCardEntry newCard)
        {
            string violatingSections = string.Empty;
            if (ProfanityFilter.ContainsProfanity(newCard.CardBody))
            {
                violatingSections += "Body ";
            }
            if (ProfanityFilter.ContainsProfanity(newCard.CardAnswer))
            {
                violatingSections += "Answer ";
            }
            return violatingSections;
        }
    }

    public class FlashCardEntry
    {
        public int ID { get; set; }

        public string Creator { get; set; }

        [StringLength(55, MinimumLength = 3)]
        public string InquizitorName { get; set; }

        public bool IsPrivate { get; set; }

        public int CardNumber { get; set; }

        [StringLength(400)]
        public string CardBody { get; set; }

        [Required]
        [StringLength(400)]
        public string CardAnswer { get; set; }

        public string CardColor { get; set; }

        public string Image { get; set; }

        public string Audio { get; set; }
    }
}
