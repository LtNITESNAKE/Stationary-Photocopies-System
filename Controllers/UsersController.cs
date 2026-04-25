using Microsoft.AspNetCore.Mvc;
using PhotocopySystem.Models;
using PhotocopySystem.Data;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace PhotocopySystem.Controllers
{
    /// <summary>
    /// GOLDEN EXAMPLE CONTROLLER (By Mujtaba)
    /// This is the master example of an MVC Controller. Team, look at how the Index and Create methods are built!
    /// </summary>
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        // 1. Inject the database context so we can read/write to the SQL Server
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: /Users/Index
        // This is the action that loads the main page showing all users.
        [Authorize(Roles = "Admin,Operator")] // ONLY Admins and Operators can see this!
        public IActionResult Index()
        {
            // Fetch all users from the real database!
            var users = _context.Users.ToList();
            return View(users); 
        }

        // GET: /Users/Login
        // This displays the beautiful Login screen
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Users/Login
        // This handles the login submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Check the database for a matching user
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
            
            if (user != null) 
            {
                // Create the secure authentication cookie
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                
                // If they are Admin/Operator, take them to User Management. Otherwise, to Home Page.
                if (user.Role == "Admin" || user.Role == "Operator")
                    return RedirectToAction(nameof(Index));
                else
                    return RedirectToAction("Index", "Home");
            }

            // If login fails, show error message
            ModelState.AddModelError("", "Invalid Email or Password. Please try again.");
            return View();
        }

        // GET: /Users/Create
        // This action just returns the empty HTML form for the user to fill out.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Users/Create
        // This action receives the submitted form data from the user.
        [HttpPost]
        [ValidateAntiForgeryToken] // Security measure to prevent Cross-Site Request Forgery (CSRF)
        public IActionResult Create(User userModel)
        {
            // 2. ModelState.IsValid checks if all the [Required] and [EmailAddress] rules in our Model are met.
            if (ModelState.IsValid)
            {
                // Save to the actual database!
                _context.Users.Add(userModel);
                _context.SaveChanges();

                // 3. If successful, redirect back to the Login page so they can sign in.
                return RedirectToAction(nameof(Login));
            }

            // 4. If there were errors (like missing name), return the same view so they can fix it.
            return View(userModel);
        }
        
        // GET: /Users/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
