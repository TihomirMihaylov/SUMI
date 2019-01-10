namespace SUMI.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SUMI.Common;
    using SUMI.Data.Models;
    using SUMI.Services.Data.Clients;
    using SUMI.Web.ViewModels.Account;

    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IClientService clientService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IClientService clientService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.clientService = clientService;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return this.Redirect("/");
            }

            this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return this.View();
        }

        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            DateTime birthday = DateTime.Parse(model.Birthday);
            var clientId = this.clientService.GetClientId(model.FirstName, model.LastName, model.UniversalCitizenNumber, birthday).GetAwaiter().GetResult();

            if (!string.IsNullOrWhiteSpace(clientId))
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UniversalCitizenNumber = model.UniversalCitizenNumber,
                    Birthday = birthday,
                    CreatedOn = DateTime.UtcNow,
                    ClientId = clientId,
                };

                var result = await this.userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Adding roles to users
                    bool isFirstUser = this.userManager.Users.Count() == 1;
                    if (isFirstUser)
                    {
                        await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                    }
                    else
                    {
                        await this.userManager.AddToRoleAsync(user, GlobalConstants.ClientRoleName);
                    }

                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return this.RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return this.View();
        }

        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync();
            return this.Redirect("/");
        }

        public IActionResult ExternalLogin()
        {
            return this.LocalRedirect("/Identity/Account/ExternalLogin");
        }
    }
}
