﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;

namespace PPTRANControlesWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ColaboradorDAL colaboradorDAL;

        public LoginModel(SignInManager<AppIdentityUser> signInManager, ILogger<LoginModel> logger, ApplicationContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            colaboradorDAL = new ColaboradorDAL(context);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                
                if (result.Succeeded)
                {
                    if (!ValidaLoginOperadorNoHorarioComercial(Input.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Login não permitido neste horário. " + DateTime.Now.Hour);
                        return Page();
                    }

                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private bool ValidaLoginOperadorNoHorarioComercial(string email)
        {
            var funcaoEncontrada = colaboradorDAL.ObterColaboradorPorEmail(email).Result.Funcao;

            if (funcaoEncontrada.ToString() == RolesNomes.Operador.ToString())
            {
                if (DateTime.Now.Hour <= 7 || DateTime.Now.Hour >= 18)                
                    return false;

                if ((DateTime.Now.Hour <= 7 || DateTime.Now.Hour >= 13) && DateTime.Today.DayOfWeek.ToString() == "Saturday")                
                    return false;

                if (DateTime.Today.DayOfWeek.ToString() == "Sunday")
                    return false;
            }

            return true;
        }
    }
}
