using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MTodo.Persistance.Context;
using MTodo.Persistance.Tables;

namespace MTodo.Controllers;

public class AuthController : Controller
{
    private MTodoContext context { set; get; }
    private ILogger<AuthController> logger { set; get; }
    public AuthController(MTodoContext context, ILogger<AuthController> logger)
    {
        this.context = context;
        this.logger = logger;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Login")]
    public async Task<IActionResult> LoginAction(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User? user = context.users.Where(p => p.Email == model.Email).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == model.Password)
                {
                    Claim[] claims = new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim(ClaimTypes.Email,user.Email)
                };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    TempData["SuccessMessage"] = "Login Successfully!";
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        TempData["ErrorMessage"] = "Login Not Successfully!";
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Register")]
    public IActionResult RegisterAction(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                User user = new User()
                {
                    UserName = model.Name,
                    Email = model.Email,
                    Address = "",
                    Password = model.Password,
                };
                context.users.Add(user);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Register Successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                logger.LogDebug(e.StackTrace);
            }

        }
        TempData["ErrorMessage"] = "Register Not Successfully!";
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Logout")]
    public async Task<IActionResult> LogoutAction()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
