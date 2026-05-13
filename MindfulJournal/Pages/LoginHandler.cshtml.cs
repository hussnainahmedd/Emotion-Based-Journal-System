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
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginHandlerModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            // Log to confirm this method is being called
            Console.WriteLine($"=== LOGIN ATTEMPT: {email} ===");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return Redirect("/login?error=invalid");

            var result = await _signInManager.PasswordSignInAsync(
                email, password, isPersistent: false, lockoutOnFailure: false);

            Console.WriteLine($"=== LOGIN RESULT: {result.Succeeded} ===");

            if (result.Succeeded)
            {
                Console.WriteLine($"=== EMAIL CHECK: '{email.Trim().ToLower()}' ===");

                if (email.Trim().ToLower() == "admin@mindfuljournal.com")
                {
                    Console.WriteLine("=== REDIRECTING TO ADMIN ===");
                    return Redirect("/admin");
                }

                return Redirect("/dashboard");
            }

            return Redirect("/login?error=invalid");
        }

        public IActionResult OnGet() => Redirect("/login");
    }
}