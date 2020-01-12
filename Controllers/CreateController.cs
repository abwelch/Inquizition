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
        private readonly UserManager<IdentityUser> _userManager;

        public CreateController(InquizitionContext dbcontext,
            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbcontext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            // Add viewdata warning if not logged in
            if (!User.Identity.IsAuthenticated)
            {
                ViewData["creationWontSave"] = "Warning: You are not logged in. Your Inquizitor will not be saved.";
            }
            else
            {
                UserInfo AuthenicatedUser = _dbContext.UserOverviewInfo.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                // This should never execute
                if (AuthenicatedUser == null)
                {
                    ViewData["createNullAuthenticatedUser"] = "Error retrieving account info.\n Suggestion: Clear your cookies.";
                    return View();
                }
                if (!AuthenicatedUser.EmailConfirmed)
                {
                    ViewData["wontShare"] = "Warning: You have not verified your email.\n" +
                        "You will be unable to share your Inquizitor with the public or with friends.";
                }
            }
            return View();
        }
    }
}