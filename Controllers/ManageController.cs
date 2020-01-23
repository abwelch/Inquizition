using Microsoft.AspNetCore.Mvc;
using Inquizition.Models;
using Inquizition.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inquizition.Controllers
{
    public class ManageController : Controller
    {
        private readonly InquizitionContext _dbContext;
        private readonly IFlashCardManager _flashCardManager;
        private readonly IColorThemeManager _colorThemeManager;
        private readonly IUserInfoManager _userInfoManager;

        public ManageController(InquizitionContext dbcontext,
            IFlashCardManager flashCardManager,
            IColorThemeManager colorThemeManager,
            IUserInfoManager userInfoManager)
        {
            _dbContext = dbcontext;
            _flashCardManager = flashCardManager;
            _colorThemeManager = colorThemeManager;
            _userInfoManager = userInfoManager;
        }

        public IActionResult Index()
        {
            // Unathenticated users cannot access this page
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            // Retrieve user info
            UserInfo user = _userInfoManager.RetrieveUserInfo(User.Identity.Name);
            if (user.TotalSets == 0)
            {
                ManageIndex emptyModel = new ManageIndex();
                ViewData["NoInquizitors"] = true;
                return View(emptyModel);
            }
            ViewData["NoInquizitors"] = false;
            ManageIndex viewModel = new ManageIndex
            {
                FlashCardInquizitorNames = _flashCardManager.RetrieveSetsAssociatedWithUser(user.Username),
                // Reassign when functionality implemented
                QuizInquizitorNames = new List<string>(),
                TwoColumnInquizitorNames = new List<string>()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ManageIndex selection)
        {
            if (selection.Type == "flashcard")
            {
                // Verify that inquiz exists (client-tampering check)
                selection.FlashCardInquizitorNames = _flashCardManager.RetrieveSetsAssociatedWithUser(User.Identity.Name);
                if (selection.FlashCardInquizitorNames.Contains(selection.Inquizitor))
                {
                    switch (selection.Operation)
                    {
                        case "View":
                            return RedirectToAction("Display", new { selection.Inquizitor, Type = "flashcard" });
                        case "Edit":
                            return RedirectToAction("Edit", new { selection.Inquizitor, Type = "flashcard" });
                        case "Delete":
                            return RedirectToAction("Delete", new { selection.Inquizitor, Type = "flashcard" });
                    }
                }
            }
            /*
            else if (selection.QuizInquizitorNames.Contains(selection.Inquizitor))
            {
                switch (selection.Operation)
                {
                    case "View":
                        return RedirectToAction("Display", new { selection.Inquizitor, Type = "quiz" });
                }
            }
            else if (selection.TwoColumnInquizitorNames.Contains(selection.Inquizitor))
            {
                switch (selection.Operation)
                {
                    case "View":
                        return View("Display", new { selection.Inquizitor, Type = "twocolumn" });
                }
            }
            */
            ViewData["SelectionError"] = "Error: the selected model and operation could not be validated.";
            return View(selection);
        }

        public IActionResult Display(string Inquizitor, string Type)
        {
            // Unathenticated users cannot access this page
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ManageDisplay viewModel = new ManageDisplay();
            switch (Type)
            {
                case "flashcard":
                    viewModel.Color = _colorThemeManager.RetrieveCardColor(Inquizitor);
                    viewModel.FlashInquizitor = _flashCardManager.RetrieveAllCards(Inquizitor);
                    return View(viewModel);
            }
            return View();
        }

        public IActionResult Edit(string Inquizitor, string Type)
        {
            // Unathenticated users cannot access this page
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            PublicDisplay viewModel = new PublicDisplay();
            switch (Type)
            {
                case "flashcard":
                    viewModel.Title = Inquizitor;
                    viewModel.Color = _colorThemeManager.RetrieveCardColor(Inquizitor);
                    viewModel.FlashInquizitor = _flashCardManager.RetrieveAllCards(Inquizitor);
                    return View(viewModel);
            }
            return View();
        }

        public IActionResult EditSpecific(string inquizitor, int cardNumber)
        {
            FlashCardEntry editCard = _flashCardManager.RetrieveCard(inquizitor, cardNumber);
            return View(editCard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSpecific(FlashCardEntry updatedCard)
        {
            if (_flashCardManager.CardContainsProfanity(updatedCard) != string.Empty)
            {
                return View();
            }
            _flashCardManager.UpdateCard(updatedCard.InquizitorName, updatedCard.CardNumber,
                updatedCard.CardBody, updatedCard.CardAnswer);
            InputFlashCard viewModel = new InputFlashCard
            {
                CardBody = updatedCard.CardBody,
                CardAnswer = updatedCard.CardAnswer,
                CardColor = _colorThemeManager.RetrieveCardColor(updatedCard.InquizitorName),
                CardNumber = updatedCard.CardNumber,
                InquizitorName = updatedCard.InquizitorName
            };
            return View("EditSuccess", viewModel);
        }

        public IActionResult DeleteSpecific(string inquizitor, int cardNumber)
        {
            _flashCardManager.DeleteCard(inquizitor, cardNumber);
            return RedirectToAction("Edit", new { Inquizitor = inquizitor, Type = "flashcard"});
        }

        public IActionResult Delete(string Inquizitor, string Type)
        {
            // Unathenticated users cannot access this page
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            PublishSummary deletionSummary = new PublishSummary
            {
                Title = Inquizitor,
                AssessmentType = Type,
                TotalEntries = _flashCardManager.TotalEntries(Inquizitor)
            };
            return View(deletionSummary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string Title)
        {
            // Ensure no client side tampering
            if (_flashCardManager.RetrieveSetsAssociatedWithUser(User.Identity.Name).Contains(Title))
            {
                _flashCardManager.DeleteInquizitor(Title);
                ViewData["inquiz"] = Title;
                return View("DeleteSuccess");
            }
            else
            {
                return RedirectToAction("Create", "ClientTampering");
            }
        }
    }
}