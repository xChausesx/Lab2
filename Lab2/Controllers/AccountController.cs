using Lab2.Areas.Identity.Data;
using Lab2.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lab2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public async Task<IActionResult> SwitchRole()
        {
            var user = await _userManager.GetUserAsync(User);

            var roles = await _userManager.GetRolesAsync(user);

            if (!await _userManager.IsInRoleAsync(user, UserRole.Admin.ToString()))
            {
                await _userManager.RemoveFromRoleAsync(user, UserRole.User.ToString());
                await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
                TempData["Message"] = "Ви в ролі admin !";
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, UserRole.Admin.ToString());
                await _userManager.AddToRoleAsync(user, UserRole.User.ToString());
                TempData["Message"] =  "Ви в ролі user !";
            }

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

    }
}
