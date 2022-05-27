using System;
using System.Threading.Tasks;
using Dive.App.Models;
using Dive.App.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dive.App.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly IUserRepository _userRepository;

        private readonly SignInManager<User> _signInManager;

        public SettingsController(IUserRepository userRepository, SignInManager<User> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpPost("/settings")]
        public async Task<RedirectResult> Save(string email, string first_name, string last_name, string current_password, string new_password, string url)
        {
            var user = await _userRepository.GetCurrentUserAsync();
            var result = await _signInManager.CheckPasswordSignInAsync(user, current_password, false);

            if (result.Succeeded)
            {
                await _userRepository.UpdateUserAsync(user.Id, email, first_name, last_name);

                if (new_password != null) await _userRepository.UpdatePasswordAsync(user, current_password, new_password);

                SetNotification("Success", "Settings updated successfully");
            }
            else
            {
                SetNotification("Error", "Current password is incorrect", false);
            }

            return Redirect(url);
        }
    }
}
