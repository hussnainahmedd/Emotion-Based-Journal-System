using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MindfulJournal.Models;

namespace MindfulJournal.Pages
{
    [IgnoreAntiforgeryToken]
    public class LoginHandlerModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginHandlerModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync(
            string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Redirect("/login?error=invalid");

            var result = await _signInManager.PasswordSignInAsync(
                email, password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
                return Redirect("/dashboard");

            return Redirect("/login?error=invalid");
        }

        public IActionResult OnGet()
        {
            return Redirect("/login");
        }
    }
}