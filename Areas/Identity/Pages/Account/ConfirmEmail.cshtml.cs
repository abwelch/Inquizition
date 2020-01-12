using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Inquizition.Data;
using Inquizition.Models;

namespace Inquizition.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly InquizitionContext _dbContext;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, InquizitionContext context)
        {
            _userManager = userManager;
            _dbContext = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                StatusMessage = "Thank you for confirming your email.";
                // Update in UserOverviewInfo
                UserInfo AuthenticatedUser = _dbContext.UserOverviewInfo.Where(u => u.Username == User.Identity.Name).SingleOrDefault();
                AuthenticatedUser.EmailConfirmed = true;
                _dbContext.UserOverviewInfo.Update(AuthenticatedUser);
                _dbContext.SaveChanges();
            }
            else
            {
                StatusMessage = "Error confirming your email!"
;            }
            return Page();
        }
    }
}
