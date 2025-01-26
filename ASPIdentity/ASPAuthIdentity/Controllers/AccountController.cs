using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using ASPAuthIdentity.Helpers;
using ASPAuthIdentity.Models;
using ASPAuthIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Engines;

namespace ASPAuthIdentity.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token = token, email = user.Email }, protocol: HttpContext.Request.Scheme);
                EmailHelper emailHelper = new EmailHelper();
                var emailReslut = await emailHelper.SendEmail(user.Email, confirmationLink, "Confirm your email");
                if (!emailReslut)
                {
                    return RedirectToAction("Error");
                }
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl, ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList() });
    }
 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result =
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                // проверяем, принадлежит ли URL приложению
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (!await _userManager.IsEmailConfirmedAsync(new User { Email = model.Email }))
            {
                ModelState.AddModelError(string.Empty, "You have not confirmed your email address.");
            }
            // else if (result.IsNotAllowed) // этот способ не пропустит всех (неподтвержденных и забаненых к примеру)
            // {
            //     ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            // }
            else
            {
                ModelState.AddModelError("", "Incorrect login and/or password");
            }
        }
        return View(model);
    }
 
    [AllowAnonymous]
    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
            new { ReturnUrl = returnUrl });
 
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
 
        return new ChallengeResult(provider, properties);
    }
    
    [AllowAnonymous]
        public async Task<IActionResult>
            ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
 
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
 
            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");
 
                return View("Login", loginViewModel);
            }
 
            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");
 
                return View("Login", loginViewModel);
            }
 
            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
 
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
 
                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await _userManager.FindByEmailAsync(email);
 
                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
 
                        await _userManager.CreateAsync(user);
                    }
 
                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
 
                    return LocalRedirect(returnUrl);
                }
 
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";
 
                return View("Error");
            }
}
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmailAsync(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return View("Error");
        }
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    }

    [HttpGet]
    public IActionResult SuccessRegistration()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword([Required] string email)
    {
        if (!ModelState.IsValid) return View(model: email);
        
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return RedirectToAction("ForgotPassword");
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var link = Url.Action("ResetPassword", "Account", new { token = token, email = user.Email }, protocol: HttpContext.Request.Scheme);
        EmailHelper emailHelper = new EmailHelper();
        bool emailResponse = await emailHelper.SendEmail(user.Email, link, "Reset your password");

        if (emailResponse)
        {
            return RedirectToAction("ForgotPasswordConfirmation", "Account");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "An error occurred, please try again later.");
        }

        return View(model: email);
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPasswordViewModel { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model: model);
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return RedirectToAction("ResetPassword", "Account");
        
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model: model);
        }
        return RedirectToAction("ResetPasswordConfirmation", "Account");
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }
}