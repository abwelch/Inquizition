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
            ManageIndex viewModel = new ManageIndex
            {
                FlashCardInquizitorNames = _flashCardManager.RetrieveSetsAssociatedWithUser(user.Username)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ManageIndex selection)
        {

            return View();
        }
    }
}