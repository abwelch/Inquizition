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
        public IActionResult InitialSetup([Bind("InquizitorName, SelectedAssessment, IsPrivate")] CreateSetup userInputs)
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

        public IActionResult FlashCards(string InquizitorName, bool IsPrivate)
        {
            InputFlashCard InputCard = new InputFlashCard
            {
                InquizitorName = InquizitorName,
                IsPrivate = IsPrivate,
                CardColor = _flashCardManager.RetrieveCardColor(InquizitorName)
            };
            return View(InputCard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        /*
         Need to figure out how to pass the viewdata values from the method above into this method
        */
        public IActionResult FlashCards([Bind("CardColor, InquizitorName, IsPrivate, CardBody, CardAnswer")] InputFlashCard newCard)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error: Invalid Input");
                return View();
            }
            string violatingAreas = _flashCardManager.CardContainsProfanity(newCard);
            if (violatingAreas != string.Empty)
            {
                ModelState.AddModelError(string.Empty, "Error: Card " + violatingAreas + "contains profanity >:(");
                return View();
            }
            // Determine color on first card and update database
            newCard.CardColor = _flashCardManager.RetrieveCardColor(newCard.InquizitorName);
            if (newCard.CardColor == null)
            {
                // User did not set a value. Set default
                if (newCard.CardColor == null)
                {
                    newCard.CardColor = "bg-primary";
                }
                AddColorTheme(newCard.CardColor, newCard.InquizitorName);
            }
            newCard.Creator = User.Identity.Name;
            // Method will call SaveChanges() and update the ColorTheme add above as well
            // Implicit upcast polymorphism in method parameter
            // This condition should never execute bc other logic is implemented to restrict creates at max cap
            if (!_flashCardManager.AddFlashCard(newCard))
            {
                ModelState.AddModelError(string.Empty, "Error: Inquizitor at max capacity");
                return View();
            }
            // Generate list of card entries for this inquizitor
            _flashCardManager.RetrieveAllCards(newCard.Inquizitor, newCard.InquizitorName);
            // Copy values to input card before newCard is out of scope
            return View(newCard);
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