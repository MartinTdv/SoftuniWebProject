namespace ConnectingPeople.Web.Areas.Identity.Pages.Account
{
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ConnectingPeople.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats;
    using SixLabors.ImageSharp.Processing;
    using System.Security.Policy;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private const string requiredFieldErrorMessage = "Полето \"{0}\" e задължително!";
        private const string stringLengthErrorMessage = "Полето \"{0}\" трябва да бъде между {2} и {1} символа.";
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnviroment = webHostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = requiredFieldErrorMessage)]
            [StringLength(14, ErrorMessage = stringLengthErrorMessage, MinimumLength = 3)]
            [Display(Name = "Псевдоним")]
            public string Username { get; set; }

            [Required(ErrorMessage = requiredFieldErrorMessage)]
            [StringLength(30, ErrorMessage = stringLengthErrorMessage, MinimumLength = 2)]
            [Display(Name = "Име")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = requiredFieldErrorMessage)]
            [StringLength(40, ErrorMessage = stringLengthErrorMessage, MinimumLength = 2)]
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }

            [Required(ErrorMessage = requiredFieldErrorMessage)]
            [EmailAddress]
            [Display(Name = "Имейл")]
            public string Email { get; set; }

            [Required(ErrorMessage = requiredFieldErrorMessage)]
            [StringLength(100, ErrorMessage = stringLengthErrorMessage, MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Парола")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Потвърди парола")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = requiredFieldErrorMessage)]
            [StringLength(30, ErrorMessage = stringLengthErrorMessage, MinimumLength = 2)]
            [Display(Name = "Населено място")]
            public string City { get; set; }

            [Required(ErrorMessage = requiredFieldErrorMessage)]
            [MaxLength(600, ErrorMessage = "Полето \"{0}\" трябва да бъде по-малко от {1} символа.")]
            [Display(Name = "Описание")]
            public string Description { get; set; }

            public IFormFile Image { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                { 
                    UserName = Input.Username, 
                    FirstName = Input.LastName, 
                    LastName = Input.LastName,
                    City = Input.City,
                    Description = Input.Description, 
                    Email = Input.Email 
                };

                if (this.Input.Image != null)
                {
                    Image image;
                    try
                    {
                        image = Image.Load(this.Input.Image.OpenReadStream());
                        var imageId = Guid.NewGuid().ToString();
                        image.Save($"{_webHostEnviroment.WebRootPath}/profilePics/{imageId}.png");
                        user.ImageName = imageId;
                    }
                    catch
                    {
                        user.ImageName = null;
                    }
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
