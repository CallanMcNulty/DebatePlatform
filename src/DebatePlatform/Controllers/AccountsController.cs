using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DebatePlatform.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using DebatePlatform.ViewModels;

namespace BasicAuthentication.Controllers
{
    public class AccountsController : Controller
    {
        private DebatePlatformContext _db;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DebatePlatformContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser current = await GetCurrentUser();
            if(current != null)
            {
                ViewBag.LoggedIn = true;
                ViewBag.UserArguments = _db.Arguments.Where(a => a.UserId == current.Id).ToList();
                return View(current);
            }
            else
            {
                ViewBag.LoggedIn = false;
                return View();
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}