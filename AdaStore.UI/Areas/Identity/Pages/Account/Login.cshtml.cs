using AdaStore.Shared.Conts;
using AdaStore.Shared.Data;
using AdaStore.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AdaStore.UI.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly ApplicationDbContext context;

        public LoginModel(SignInManager<User> signInManager,
            UserManager<User> userManager,
            ApplicationDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl = Url.Content("~/");

            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid && Validation())
            {
                var user = await context.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserName == Input.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, Conts.IncorrectCredentials);
                    return Page();
                }

                var roles = await userManager.GetRolesAsync(user);

                if (!roles.Any(r=>r == Conts.Admin || r == Conts.Buyer))
                {
                    ModelState.AddModelError(string.Empty, Conts.IncorrectCredentials);
                    return Page();
                }

                string displayName = string.Empty;

                displayName = user.Name;

                var claimDisplayName = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "DisplayName");

                var userInManager = await userManager.FindByIdAsync(user.Id.ToString());

                if (claimDisplayName == null)
                    await userManager.AddClaimAsync(userInManager, new Claim("DisplayName", displayName));
                else
                    await userManager.ReplaceClaimAsync(userInManager, claimDisplayName, new Claim("DisplayName", displayName));

                var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.Rememberme, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return LocalRedirect(ReturnUrl);
                }
            }

            ModelState.AddModelError(string.Empty, Conts.IncorrectCredentials);
            return Page();
        }

        private bool Validation()
        {
            bool isOk = true;

            if (string.IsNullOrEmpty(Input.Email))
            {
                ModelState.AddModelError("Input.Email", "El correo es obligatorio");
                isOk = false;
            }
            else if (!Regex.IsMatch(Input.Email.Trim(), @"^([+\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))
            {
                ModelState.AddModelError("Input.Email", "El correo no tiene el formato correcto");
                isOk = false;
            }

            if (string.IsNullOrEmpty(Input.Password))
            {
                ModelState.AddModelError("Input.Password", "La contraseña es obligatoria");
                isOk = false;
            }
            else if (isOk && Input.Password.Length < 7)
            {
                ModelState.AddModelError(string.Empty, Conts.IncorrectCredentials);
                isOk = false;
            }

            return isOk;
        }

        public class InputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool Rememberme { get; set; }
        }
    }
}
