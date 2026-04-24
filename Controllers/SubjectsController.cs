using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // I have injected the database for you!
        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Teacher Role Developer: You are managing the Subjects that Teachers teach!

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Shows all Subjects.
        public IActionResult Index()
        {
            var subjects = _context.Subjects.ToList();
            return View(subjects);
        }

        // 2. Create() - GET
        // TODO: Return the empty form view.
        
        // 3. Create(Subject subject) - POST
        // TODO: Save to _context.Subjects and redirect to Index.
    }
}
