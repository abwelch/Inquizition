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
        private readonly IFlashCards _flashCardManager;
        public CreateController(InquizitionContext dbcontext, IFlashCards flashCardManager)
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
                        userInputs.SelectedAssessment,
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

        public IActionResult FlashCards(string InquizitorName, string SelectedAssessment, bool IsPrivate)
        {
            ViewData["InquizitorName"] = InquizitorName;
            ViewData["SelectedAssessment"] = SelectedAssessment;
            ViewData["IsPrivate"] = IsPrivate;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FlashCards(int? cardNumber,
            [Bind("CardBody,CardAnswer")] FlashCardEntry newCard)
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