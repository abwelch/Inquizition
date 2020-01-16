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
        public CreateController(InquizitionContext dbcontext, IFlashCardManager flashCardManager)
        {
            _dbContext = dbcontext;
            _flashCardManager = flashCardManager;
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
            if (newCard.ConfirmedPublish)
            {

            }
            ViewData["Profanity"] = false;
            if (!ModelState.IsValid)
            {
                _flashCardManager.RetrieveAllCards(newCard.Inquizitor, newCard.InquizitorName);
                return View(newCard);
            }
            // Assign random value to users not logged in
            // Will only execute once and then pass random value each subsequent time from view
            if (newCard.Creator == null)
            {
                Random rnd = new Random();
                newCard.Creator = rnd.Next(1, 10000000).ToString();
            }
            if (newCard.FirstCard == true)
            {
                // Track published
                AddPublishedTab(newCard.InquizitorName);
                if (newCard.CardColor == null)
                {
                    // Set default color
                    newCard.CardColor = "bg-primary";
                }
                newCard.FirstCard = false;
                AddColorTheme(newCard.CardColor, newCard.InquizitorName);
            }
            string violatingAreas = _flashCardManager.CardContainsProfanity(newCard);
            if (violatingAreas != string.Empty)
            {
                ViewData["Profanity"] = true;
                _flashCardManager.RetrieveAllCards(newCard.Inquizitor, newCard.InquizitorName);
                return View(newCard);
            }
            // Implicit upcast polymorphism in method parameter
            // This condition should never execute bc other logic is implemented to restrict creates at max cap
            if (!_flashCardManager.AddFlashCard(newCard))
            {
                ViewData["MaxCap"] = true;
                _flashCardManager.RetrieveAllCards(newCard.Inquizitor, newCard.InquizitorName);
                return View(newCard);
            }
            // Generate list of card entries for this inquizitor
            _flashCardManager.RetrieveAllCards(newCard.Inquizitor, newCard.InquizitorName);
            return View(newCard);
        }

        private void AddPublishedTab(string name)
        {
            Publish entry = new Publish
            {
                InquizitorName = name,
                Published = false,
            };
            _dbContext.Publish.Add(entry);
            _dbContext.SaveChanges();
        }

        private void AddColorTheme(string color, string name)
        {
            ColorTheme newColorEntry = new ColorTheme
            {
                Color = color,
                InquizitorName = name
            };
            _dbContext.ColorTheme.Add(newColorEntry);
            _dbContext.SaveChanges();
        }

        public IActionResult MidSummary(string type, string inquizName)
        {

            return View();
        }

        public IActionResult FinalSummary(string type, string inquizName)
        {

            return View();
        }

        public void Authenticate()
        {
            // Add viewdata warning if not logged in
            if (!User.Identity.IsAuthenticated)
            {
                ViewData["creationWontSave"] = "Warning: You are not logged in. Your Inquizitor will not be saved.";
            }
            else
            {
                UserInfo AuthenicatedUser = _dbContext.UserOverviewInfo.FirstOrDefault(u => u.Username == User.Identity.Name);
                // This should never execute
                if (AuthenicatedUser == null)
                {
                    ViewData["createNullAuthenticatedUser"] = "Error retrieving account info.\n Suggestion: Clear your cookies.";
                    return;
                }
                if (!AuthenicatedUser.EmailConfirmed)
                {
                    ViewData["wontShare"] = "Warning: You have not verified your email.\n" +
                        "You will be unable to share your Inquizitor with the public or with friends.";
                }
            }
        }
    }
}