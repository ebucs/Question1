using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Question1.Data;
using Question1.Models;
using Question1.ViewModels;
using System.Diagnostics;

namespace Question1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Person> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<Person> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("User"))
                {
                    return RedirectToAction("InfoPage", "Home");
                }
            }

            // Handle the case where the user is not authenticated
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> InfoPage()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            // Get the user's info from the database
            var userInfo = await _context.Info.FirstOrDefaultAsync(i => i.PersonId == user.Id);

            if (userInfo != null)
            {
                // Map the user's info to the EditInfoViewModel
                var model = new EditInfoViewModel
                {
                    PersonId = userInfo.PersonId,
                    Name = user.Name,
                    Surname = user.Surname,
                    TellNo = userInfo.TellNo,
                    CellNo = user.PhoneNumber,
                    AddressLine1 = userInfo.AddressLine1,
                    AddressLine2 = userInfo.AddressLine2,
                    AddressLine3 = userInfo.AddressLine3,
                    AddressCode = userInfo.AddressCode,
                    PostalAddress1 = userInfo.PostalAddress1,
                    PostalAddress2 = userInfo.PostalAddress2,
                    PostalCode = userInfo.PostalCode,
                    Person = user
                };

                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> InfoPage(EditInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // ModelState is invalid, return the view with validation errors
                return View(model);
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            // Update the user's info
            var userInfo = await _context.Info.FirstOrDefaultAsync(i => i.PersonId == user.Id);
            if (userInfo != null)
            {
                userInfo.PersonId = model.PersonId;
                userInfo.TellNo = model.TellNo;
                userInfo.AddressLine1 = model.AddressLine1;
                userInfo.AddressLine2 = model.AddressLine2;
                userInfo.AddressLine3 = model.AddressLine3;
                userInfo.AddressCode = model.AddressCode;
                userInfo.PostalAddress1 = model.PostalAddress1;
                userInfo.PostalAddress2 = model.PostalAddress2;
                userInfo.PostalCode = model.PostalCode;

                _context.Info.Update(userInfo);
                await _context.SaveChangesAsync();

                // Update user's phone number
                user.PhoneNumber = model.CellNo;
                await _userManager.UpdateAsync(user);

                // Set success message
                TempData["SuccessMessage"] = "Info updated successfully.";
            }
            else
            {
                // Handle case where user info is not found
                return NotFound();
            }

            return RedirectToAction("InfoPage");
        }


        [Authorize(Roles = "User")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // ModelState is invalid, return the view with validation errors
                return View(model);
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            // Change user's password
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                // Handle password change failure
                ModelState.AddModelError(string.Empty, "Password change failed. Please check your current password.");
                return View(model);
            }
            // Set success message
            TempData["SuccessMessage"] = "Password updated successfully.";

            // Password changed successfully
            return RedirectToAction("InfoPage");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
