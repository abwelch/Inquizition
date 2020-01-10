using System;
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
        private UserInfo AuthenticatedUser { get; set; }

        public HomeController(ILogger<HomeController> logger,
            InquizitionContext context,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                // Retrieve user info from table
                AuthenticatedUser = _dbContext.UserOverviewInfo.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                if (AuthenticatedUser == null)
                {
                    // Band-aid fix to bug if cookies keep user logged in but is not identified
                    var tempUser = _userManager.GetUserAsync(User).Result;
                    if (tempUser == null)
                    {
                        return View();
                    }
                    AuthenticatedUser = new UserInfo
                    {
                        // Add entry to table
                        Username = User.Identity.Name,
                        IntroCompleted = false,
                        EmailConfirmed = _userManager.IsEmailConfirmedAsync(tempUser).Result,
                        TotalFriends = 0,
                        TotalSets = 0,
                        TotalBookmarks = 0
                    };
                    _dbContext.UserOverviewInfo.Add(AuthenticatedUser);
                    _dbContext.SaveChanges();
                }
                return View(AuthenticatedUser);              
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
