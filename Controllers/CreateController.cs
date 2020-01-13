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

        [BindProperty]
        public FlashCardEntry NewCard { get; set; }
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

        [HttpPost]
        public IActionResult InitialSetup(int? id)
        {
            // Determine view with enum
            return View();
        }

        public IActionResult FlashCards(string inquizName)
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FlashCards(string inquizName, int? cardNumber)
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
    }
}