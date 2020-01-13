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
        private FlashCards FlashCardManager {get; set;}
        public CreateController(InquizitionContext dbcontext)
        {
            _dbContext = dbcontext;
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
        public IActionResult InitialSetup([Bind("AssessmentName, SelectedAssessment, IsPrivate")] CreateSetup userInputs)
        {
            Authenticate();
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error: Invalid Input");
                return View();
            }
            if (ProfanityFilter.ContainsProfanity(userInputs.AssessmentName))
            {
                ModelState.AddModelError(string.Empty, "Error: Inquizitor name contains profanity >:(");
                return View();
            }
            if (FlashCardManager.InquizitorNameAvailable(userInputs.AssessmentName))
            {
                ModelState.AddModelError(string.Empty, "Error: Inquizitor name is already in use.");
                return View();
            }
            switch (userInputs.SelectedAssessment)
            {
                case "flashcards":
                    return View("FlashCards", userInputs);
                case "quiz":
                    return View("Quiz", userInputs);
                case "twocolumn":
                    return View("TwoColumnList", userInputs);
                default:
                    // Add an error message
                    return View();
            }
        }

        public IActionResult FlashCards()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FlashCards(string inquizName, int? cardNumber,
            [Bind("AssessmentName, SelectedAssessment, IsPrivate")] FlashCardEntry newCard)
        {
            return View();
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