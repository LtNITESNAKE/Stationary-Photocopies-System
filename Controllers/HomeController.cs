using Microsoft.AspNetCore.Mvc;
using PhotocopySystem.Models;
using PhotocopySystem.Data;
using System.Diagnostics;

namespace PhotocopySystem.Controllers;

    [Microsoft.AspNetCore.Authorization.Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // ACTIVELY FETCHING LIVE STATS FROM DB
            ViewBag.UserCount = _context.Users.Count();
            ViewBag.JobCount = _context.PrintJobs.Count(j => j.Status == "Queued" || j.Status == "Printing");
            ViewBag.OrderCount = _context.Orders.Count();
            
            return View();
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
