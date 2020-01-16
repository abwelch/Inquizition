using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Inquizition.Models;
using Inquizition.Data;

namespace Inquizition.Controllers
{
    public class ManageController : Controller
    {
        private readonly InquizitionContext _dbContext;
        private readonly IFlashCardManager _flashCardManager;
        public ManageController(InquizitionContext dbcontext, IFlashCardManager flashCardManager)
        {
            _dbContext = dbcontext;
            _flashCardManager = flashCardManager;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}