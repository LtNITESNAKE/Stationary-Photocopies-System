using Microsoft.AspNetCore.Mvc;
using PhotocopySystem.Models;
using PhotocopySystem.Data;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PhotocopySystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users); 
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            
            if (user != null) 
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("Balance", user.Balance.ToString("F2"))
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                
                // Bug 7 fix: Role-based redirect after login
                return user.Role switch
                {
                    "Teacher" => RedirectToAction("Index", "Notes"),
                    "Admin"   => RedirectToAction("Index", "Users"),
                    _         => RedirectToAction("Index", "Home")  // Student
                };
            }

            ModelState.AddModelError("", "Invalid Email or Password.");
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(User userModel)
        {
            if (ModelState.IsValid)
            {
                userModel.Role = "Student"; // Default role for self-signup
                _context.Users.Add(userModel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Login));
            }
            return View(userModel);
        }

        // Feature 3: Admin can create users with specific roles
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string> { "Admin", "Teacher", "Student" };
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User userModel)
        {
            if (_context.Users.Any(u => u.Email == userModel.Email))
            {
                ModelState.AddModelError("Email", "Email already registered.");
                ViewBag.Roles = new List<string> { "Admin", "Teacher", "Student" };
                return View(userModel);
            }

            if (ModelState.IsValid)
            {
                _context.Users.Add(userModel);
                _context.SaveChanges();
                TempData["Success"] = $"User '{userModel.FullName}' created as {userModel.Role}.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = new List<string> { "Admin", "Teacher", "Student" };
            return View(userModel);
        }
        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
