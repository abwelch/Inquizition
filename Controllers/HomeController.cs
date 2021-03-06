﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Inquizition.Models;
using Inquizition.Data;
using Microsoft.AspNetCore.Identity;

namespace Inquizition.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InquizitionContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFlashCardManager _flashCardManager;
        private readonly IColorThemeManager _colorThemeManager;

        private UserInfo AuthenticatedUser { get; set; }

        public HomeController(ILogger<HomeController> logger,
            InquizitionContext context,
            UserManager<IdentityUser> userManager,
            IFlashCardManager flashCardManager,
            IColorThemeManager colorThemeManager)
        {
            _logger = logger;
            _dbContext = context;
            _userManager = userManager;
            _flashCardManager = flashCardManager;
            _colorThemeManager = colorThemeManager;
        }

        public async Task<IActionResult> Index()
        {
             /* 
             * This is shit design and flashcard view should dynamically generate new input card forms and then send
             * all forms using [frombody] to a list in the controller parameter. This would eliminate much of the database
             * access and elimnate need to continously call this function to remove unassigned cards
             */
            _flashCardManager.ClearUnathenticatedCards();
            _colorThemeManager.ClearUnathenticatedTheme(_flashCardManager.DeleteFlagUsername);

            if (User.Identity.IsAuthenticated)
            {
                AuthenticatedUser = _dbContext.UserOverviewInfo.FirstOrDefault(u => u.Username == User.Identity.Name);
                // User has just registered and is not yet included in UserOverviewInfo table
                if (AuthenticatedUser == null)
                {
                    var tempUser = await _userManager.GetUserAsync(User);
                    // This should never execute
                    if (tempUser == null)
                    {
                        ViewData["homeNullAuthenticatedUser"] = "Error retrieving account info.\n Suggestion: Clear your cookies.";
                        return View();
                    }
                    AuthenticatedUser = new UserInfo()
                    {
                        // Add user entry
                        // Values can be assumed b/c submitting registration form immediately redirects to homepage
                        Username = User.Identity.Name,
                        EmailConfirmed = false,
                        IntroCompleted = false,
                        TotalFriends = 0,
                        TotalSets = 0,
                        TotalBookmarks = 0,
                        ReportedInstances = 0,
                        Banned = false
                    };
                    _dbContext.UserOverviewInfo.Add(AuthenticatedUser);
                    _dbContext.SaveChanges();
                }
                return View(AuthenticatedUser);              
            }
            return View();
        }

        public IActionResult PublicViewer(string Inquizitor, string Creator, string Type)
        {
            PublicDisplay viewModel = new PublicDisplay();
            switch (Type)
            {
                case "flashcard":
                    viewModel.Color = _colorThemeManager.RetrieveCardColor(Inquizitor);
                    viewModel.FlashInquizitor = _flashCardManager.RetrieveAllCards(Inquizitor);
                    viewModel.Creator = Creator;
                    viewModel.Title = Inquizitor;
                    return View(viewModel);
            }
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
