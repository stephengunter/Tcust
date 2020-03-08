using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IdentityApp.Models;
using IdentityApp.Views;
using ApplicationCore.Helpers;
using IdentityApp.Services;

namespace IdentityApp.Areas.Api.Controllers
{
    [Area("Api")]
    public class UsersController : IdentityApp.Controllers.BaseController
    {
        private readonly IUserService userService;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IHostingEnvironment environment, IOptions<AppSettings> settings, IUserService userService) 
            : base(userManager, roleManager, environment, settings)

        {
            this.userService = userService;
        }


        AppSettings AppSettings => Settings.Value;

        [HttpPost("[area]/[controller]/{key}")]
        public async Task<IActionResult> Store(string key, [FromBody] IdentityUserViewModel model)
        {
            if (key != AppSettings.AdminKey)
            {
                ModelState.AddModelError("key", "權限不足");
                return BadRequest(ModelState);
            }

            string email = model.email.Trim();

            var existUser = userService.GetUserByEmail(email);

            if (existUser == null)
            {
                var emailPart = email.Split("@")[1];
                string role = emailPart == "tcust.edu.tw" ? "Staff" : "Student";

                //新增
                var profile = new Profile
                {
                    Fullname = model.name
                };

                var newUser = await CreateUserAsync(email, role, profile);

                return Ok(newUser.Id);
            }
            else 
            {
                existUser.Profile.Fullname = model.name;
                //更新
                await userService.UpdateUserAsync(existUser);

                return Ok(existUser.Id);
            }
           
        }

       
    }
}