using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inquizition.Data;
using Inquizition.Models;
using Microsoft.AspNetCore.Identity;

namespace Inquizition.Controllers
{
    public class CreateController : Controller
    {
        private readonly InquizitionContext _dbContext;
        private readonly IFlashCardManager _flashCardManager;
        private readonly IColorThemeManager _colorThemeManager;

        public CreateController(InquizitionContext dbcontext, 
            IFlashCardManager flashCardManager, 
            IColorThemeManager colorThemeManager)
        {
            _dbContext = dbcontext;
            _flashCardManager = flashCardManager;
            _colorThemeManager = colorThemeManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InitialSetup()
        {
            Authenticate();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InitialSetup(CreateSetup userInputs)
        {
            Authenticate();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error: Invalid Input");
                return View();
            }
            if (ProfanityFilter.ContainsProfanity(userInputs.InquizitorName))
            {
                ModelState.AddModelError(string.Empty, "Error: Inquizitor name contains profanity >:(");
                return View();
            }
            if (!_flashCardManager.InquizitorNameAvailable(userInputs.InquizitorName))
            {
                ModelState.AddModelError(string.Empty, "Error: Inquizitor name is already in use.");
                return View();
            }
            switch (userInputs.SelectedAssessment)
            {
                case "flashcards":
                    return RedirectToAction("FlashCards", new
                    {
                        userInputs.InquizitorName,
                        userInputs.IsPrivate
                    });
                case "quiz":
                    return View("Quiz", userInputs);
                case "twocolumn":
                    return View("TwoColumnList", userInputs);
                default:
                    // Add an error message
                    return View();
            }
        }

        [HttpGet]
        public IActionResult FlashCards(string InquizitorName, bool IsPrivate)
        {
            ViewData["Profanity"] = false;
            InputFlashCard InputCard = new InputFlashCard
            {
                InquizitorName = InquizitorName,
                IsPrivate = IsPrivate,
                FirstCard = true
            };
            return View(InputCard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FlashCards(InputFlashCard newCard)
        {
            Authenticate();
            ViewData["Profanity"] = false;
            if (!ModelState.IsValid)
            {
                newCard.Inquizitor = _flashCardManager.RetrieveAllCards(newCard.InquizitorName);
                return View(newCard);
            }
            // Assign the delete flag username to unathenticated users
            // Will only execute once and then pass delete flag value each subsequent time from view
            if (newCard.Creator == null)
            {
                newCard.Creator = _flashCardManager.DeleteFlagUsername;
            }
            if (newCard.FirstCard)
            {
                // Ensure that this aligns with database records (validation against client-side tampering)
                if (_flashCardManager.TotalEntries(newCard.InquizitorName) > 0)
                {
                    return View("ClientTampering");
                }
                if (newCard.CardColor == null)
                {
                    // Set default color
                    newCard.CardColor = "bg-primary";
                }
                newCard.FirstCard = false;
                _colorThemeManager.AddColorTheme(newCard.CardColor, newCard.InquizitorName, newCard.Creator);
            }
            string violatingAreas = _flashCardManager.CardContainsProfanity(newCard);
            if (violatingAreas != string.Empty)
            {
                ViewData["Profanity"] = true;
                newCard.Inquizitor = _flashCardManager.RetrieveAllCards(newCard.InquizitorName);
                return View(newCard);
            }
            // Implicit upcast polymorphism in method parameter
            // This condition should never execute bc other logic is implemented to restrict creates at max cap
            if (!_flashCardManager.AddFlashCard(newCard))
            {
                newCard.Inquizitor = _flashCardManager.RetrieveAllCards(newCard.InquizitorName);
                return View(newCard);
            }
            // Generate list of card entries for this inquizitor
            newCard.Inquizitor = _flashCardManager.RetrieveAllCards(newCard.InquizitorName);
            return View(newCard);
        }

        public IActionResult PublishSummary(string inquizName, int total, string creator, string type)
        {
            PublishSummary summary = new PublishSummary
            {
                Title = inquizName,
                AssessmentType = type,
                TotalEntries = total,
                Creator = creator
            };
            if (!(creator == _flashCardManager.DeleteFlagUsername))
            {
                var user = _dbContext.UserOverviewInfo.FirstOrDefault(u => u.Username == User.Identity.Name);
                user.TotalSets++;
                _dbContext.UserOverviewInfo.Update(user);
                _dbContext.SaveChanges();
            }
            return View(summary);
        }

        public void Authenticate()
        {
            // Add viewdata warning if not logged in
            if (!User.Identity.IsAuthenticated)
            {
                ViewData["creationWontSave"] = "Warning: You are not logged in.";
            }
            else
            {
                UserInfo AuthenicatedUser = _dbContext.UserOverviewInfo.FirstOrDefault(u => u.Username == User.Identity.Name);
                // This should never execute
                if (AuthenicatedUser == null)
                {
                    ViewData["createNullAuthenticatedUser"] = "Error retrieving account info.\n Suggestion: Clear your cookies.";
                }
            }
        }
    }
}