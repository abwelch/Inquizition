﻿using Microsoft.AspNetCore.Mvc;
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
            // Validate chosen inquizitor and redirect
            if (selection.FlashCardInquizitorNames.Contains(selection.Inquizitor))
            {
                switch (selection.Operation)
                {
                    case "View":
                        return View("Display", new { selection.Inquizitor, Type = "flashcard" });
                }
            }
            else if (selection.QuizInquizitorNames.Contains(selection.Inquizitor))
            {
                switch (selection.Operation)
                {
                    case "View":
                        return View("Display", new { selection.Inquizitor, Type = "quiz" });
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
            
            ViewData["SelectionError"] = "Error: the selected model and operation could not be validated.";
            return View(selection);
        }

        public IActionResult Display(string Inquizitor, string Type)
        {
            ManageDisplay viewModel = new ManageDisplay();
            switch (Type)
            {
                case "flashcard":
                    viewModel.Color = _colorThemeManager.RetrieveCardColor(Inquizitor);
                    _flashCardManager.RetrieveAllCards(viewModel.FlashInquizitor, Inquizitor);
                    return View(viewModel);
            }
            return View();
        }
    }
}