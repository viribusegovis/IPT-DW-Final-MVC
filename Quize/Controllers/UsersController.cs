using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quize.Models;
using System.Threading.Tasks;

namespace Quize.Controllers
{
    /// <summary>
    /// Controller for managing Users. Only accessible to authorized users (admins).
    /// </summary>
    [Authorize] // Ensure only admins can access this controller
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the UsersController.
        /// </summary>
        /// <param name="userManager">The UserManager for managing IdentityUser instances.</param>
        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Displays a list of all users.
        /// </summary>
        /// <returns>A view containing a list of all users.</returns>
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        /// <summary>
        /// Displays details of a specific user, including their roles and claims.
        /// </summary>
        /// <param name="id">The ID of the user to display.</param>
        /// <returns>A view containing the details of the specified user.</returns>
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            ViewBag.Roles = roles;
            ViewBag.Claims = claims;

            return View(user);
        }

        /// <summary>
        /// Displays the delete confirmation page for a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A view asking for confirmation to delete the specified user.</returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Processes the user deletion after confirmation.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>Redirects to the Index action after successful deletion, or returns to the Delete view if there are errors.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(user);
        }
    }
}
