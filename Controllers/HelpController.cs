﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inquizition.Models;
using Inquizition.Data;
using Microsoft.AspNetCore.Identity;


namespace Inquizition.Controllers
{
    public class HelpController : Controller
    {
        private readonly InquizitionContext _dbContext;
        private UserInfo AuthenticatedUser { get; set; }

        public HelpController(InquizitionContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Retrieve user info from table
                AuthenticatedUser = _dbContext.UserOverviewInfo.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                // This should never execute
                if (AuthenticatedUser == null)
                {
                    ViewData["homeNullAuthenticatedUser"] = "Error retrieving account info.\n Suggestion: Clear your cookies.";
                    return View();
                }
                return View(AuthenticatedUser);
            }


            return View();
        }

        public IActionResult Reportbug()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReportBug(ReportBug submission)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User.Identity.IsAuthenticated)
            {
                submission.User = User.Identity.Name;
            }
            else
            {
                submission.User = "Unknown";
            }
            _dbContext.BugReports.Add(submission);
            _dbContext.SaveChanges();
            return View("SubmissionSuccess");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}