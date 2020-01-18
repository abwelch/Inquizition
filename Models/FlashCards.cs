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
        public string DeleteFlagUsername { get; }

        public bool AddFlashCard(FlashCardEntry card);

        public bool InquizitorNameAvailable(string input);

        public string CardContainsProfanity(FlashCardEntry card);

        public int TotalEntries(string inquizName);

        public List<FlashCardEntry> RetrieveAllCards(string inquizName);

        public void ClearUnathenticatedCards();

        public List<string> RetrieveSetsAssociatedWithUser(string username);
    }

    public class FlashCardManager : IFlashCardManager
    {
        private readonly InquizitionContext _dbContext;
        private const int MaxCapacity = 50;
        private const string deleteFlagUsername = "$$$!!!$$$";

        public string DeleteFlagUsername
        {
            get { return deleteFlagUsername; }
        }

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

        public List<FlashCardEntry> RetrieveAllCards(string inquizName)
        {
            List<FlashCardEntry> Inquizitor = new List<FlashCardEntry>();
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
            return Inquizitor;
        }

        public void ClearUnathenticatedCards()
        {
            // Removes entries from FlashCard table
            var toDeleteCards = _dbContext.FlashCards.Where(f => f.Creator == deleteFlagUsername);
            foreach (FlashCardEntry f in toDeleteCards)
            {
                _dbContext.FlashCards.Remove(f);
            }
            _dbContext.SaveChanges();
        }

        // Create a list of unique inquizitors associated with the user
        public List<string> RetrieveSetsAssociatedWithUser(string username) =>
            _dbContext.FlashCards.Where(f => f.Creator == username)
                .Select(f => f.InquizitorName).Distinct().ToList();
    }

    // Table schema class
    public class FlashCardEntry
    {
        public int ID { get; set; }

        public string Creator { get; set; }

        public string InquizitorName { get; set; }

        public bool IsPrivate { get; set; }

        public int CardNumber { get; set; }

        [Required]
        [StringLength(400, ErrorMessage = "Card body can be no more than 400 characters.")]
        public string CardBody { get; set; }

        [Required]
        [StringLength(400, ErrorMessage = "Card answer can be no more than 400 characters.")]
        public string CardAnswer { get; set; }
    }

    // Class utilized for view input processing
    public class InputFlashCard : FlashCardEntry
    {
        public string CardColor { get; set; }

        public bool FirstCard { get; set; }

        public bool ConfirmedPublish { get; set; }

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
