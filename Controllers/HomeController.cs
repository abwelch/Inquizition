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

        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
