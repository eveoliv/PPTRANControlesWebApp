using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using PPTRANControlesWebApp.Areas.Identity.Data;
using System;

namespace PPTRANControlesWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public ResetPasswordModel(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(11, ErrorMessage = "A Senha deve ter no mínimo {2} e no maximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Senha")]
            [Compare("Password", ErrorMessage = "A senha e a confirmação estão diferentes.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            try
            {
                Input = new InputModel
                {
                    Email = _userManager.FindByIdAsync(_userManager.GetUserAsync(User).Result.Id).Result.Email,
                    Code = code
                };
                return Page();

            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                return RedirectToPage("./Login");
            }

            //if (code == null)
            //{
            //    return BadRequest("A code must be supplied for password reset.");
            //}
            //else
            //{                              
            //    Input = new InputModel
            //    {                  
            //        Code = code
            //    };
            //    return Page();
            //}
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var mail = _userManager.GetUserAsync(User).Result.Email;

            //var user = await _userManager.FindByEmailAsync(model.Input.Email);
            if (user == null || Input.Password == null)
            {
                Input = new InputModel { Email = mail };
                // Don't reveal that the user does not exist
                //return RedirectToPage("./ResetPasswordConfirmation");
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, token, Input.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToPage("./Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            Input = new InputModel { Email = mail };

            return Page();
        }
    }
}
