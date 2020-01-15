using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Inquizition.Data;

namespace Inquizition.Models
{
    public interface IFlashCardManager
    {
        public string RetrieveCardColor(string inquizName);

        public bool AddFlashCard(FlashCardEntry card);

        public bool InquizitorNameAvailable(string input);

        public string CardContainsProfanity(FlashCardEntry card);

        public int TotalEntries(string inquizName);

        public void RetrieveAllCards(List<FlashCardEntry> Inquizitor, string inquizName);
    }

    public class FlashCardManager : IFlashCardManager
    {
        private readonly InquizitionContext _dbContext;
        private const int MaxCapacity = 50;

        public FlashCardManager(InquizitionContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddFlashCard(FlashCardEntry newCard)
        {
            int total = TotalEntries(newCard.InquizitorName);
            if (total >= MaxCapacity)
            {
                return false;
            }
            newCard.CardNumber = total + 1;
            _dbContext.FlashCards.Add(newCard);
            _dbContext.SaveChanges();
            return true;
        }

        public string RetrieveCardColor(string inquziName)
        {
            var entry = _dbContext.ColorTheme.FirstOrDefault(c => c.InquizitorName == inquziName);
            if (entry == null)
                return string.Empty;
           return entry.Color;
        }

        public int TotalEntries(string inquizName) =>
            _dbContext.FlashCards.Count(f => f.InquizitorName == inquizName);

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

        public void RetrieveAllCards(List<FlashCardEntry> Inquizitor, string inquizName)
        {
            // Ensure empty before retrieving all elements in inquizitor
            if (Inquizitor.Count != 0)
            {
                Inquizitor.Clear();
            }
            foreach (FlashCardEntry f in _dbContext.FlashCards)
            {
                if (f.InquizitorName == inquizName)
                {
                    Inquizitor.Add(f);
                }
            }
        }
    }

    // Table schema class
    public class FlashCardEntry
    {
        public int ID { get; set; }

        public string Creator { get; set; }

        [StringLength(55, MinimumLength = 3)]
        public string InquizitorName { get; set; }

        public bool IsPrivate { get; set; }

        public int CardNumber { get; set; }

        [Required]
        [StringLength(400)]
        public string CardBody { get; set; }

        [Required]
        [StringLength(400)]
        public string CardAnswer { get; set; }
    }

    // Class utilized for view input processing
    public class InputFlashCard : FlashCardEntry
    {
        public string CardColor { get; set; }

        public bool FirstCard { get; set; }

        public List<FlashCardEntry> Inquizitor { get; set; }

        public InputFlashCard()
        {
            Inquizitor = new List<FlashCardEntry>();
        }

        public InputFlashCard ShallowCopy()
        {
            return (InputFlashCard)this.MemberwiseClone();
        }
    }
}
