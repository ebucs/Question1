using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Question1.Data;
using Question1.Models;
using Question1.ViewModels;

namespace Question1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user == null)
            {
                // User not found
                TempData["Error"] = "User not found!";
                return View(loginViewModel);
            }

            // Person is found, check password
            var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                user.LastLogin = DateTime.Now;
                // Redirect to the appropriate page based on the user's role
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Password is incorrect or sign-in failed for some other reason
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
