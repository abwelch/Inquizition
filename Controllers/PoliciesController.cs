using Microsoft.AspNetCore.Mvc;

namespace Inquizition.Controllers
{
    public class PoliciesController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult TermsOfUse()
        {
            return View();
        }
    }
}