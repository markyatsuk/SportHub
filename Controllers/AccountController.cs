using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportHub.Models.ViewModels;

namespace SportHub.Controllers;
/*
 here Attribute Routing is used. it is suited for: 
 - admin panels;
 - API;
 - secured panels.
 usually using simple routes.
 */
[Authorize]
[Route("Account")]
public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
{
    // Guard clauses: ensure required dependencies are injected before controller is used
    private readonly UserManager<IdentityUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly SignInManager<IdentityUser> _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    
    // GET action to display Login form
    // [AllowAnonymous] overrides class-level [Authorize] — unauthenticated users must be able to reach the login page
    [Route("Login")]
    [AllowAnonymous]
    public ViewResult Login(string returnUrl = "/")
    {
        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    // POST action to display Login view and pass LoginViewModel
    // [AllowAnonymous] overrides class-level [Authorize]
    
    /* [ValidateAntiForgeryToken] protects against CSRF — server validates hidden token
    // that was embedded in the form during GET request. Forged requests from other sites
    // won't have this token and will be rejected with 400 Bad Request. */
    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        // local function to avoid repeating error handling
        IActionResult InvalidLogin()
        {
            // show message if credentials are incorrect
            ModelState.AddModelError(string.Empty, "Invalid name or password.");
            // return Login view with validation errors
            return View(loginViewModel);
        }
        
        // check form filling
        if (!ModelState.IsValid) return View(loginViewModel);
        
        //check if name exists
        if (loginViewModel.Name == null) return InvalidLogin();
        
        // find user by name
        IdentityUser? user = await userManager.FindByNameAsync(loginViewModel.Name);
        
        // check if user exists
        if (user == null) return InvalidLogin();
        
        // sign out if there was a logged user
        await signInManager.SignOutAsync();
        
        // check for password matching
        if (loginViewModel.Password != null && 
            (await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false)).Succeeded)
        {
            // redirect to the Products action of Admin controller
            return RedirectToAction("Products", "Admin");
        }
        
        return InvalidLogin();
    }

    // GET action to log out
    [Route("Logout")]
    public async Task<IActionResult> Logout(string returnUrl = "/")
    {
        // log out
        await signInManager.SignOutAsync();
        // redirect to the page we were or home page by default
        return Redirect(returnUrl);
    }

}