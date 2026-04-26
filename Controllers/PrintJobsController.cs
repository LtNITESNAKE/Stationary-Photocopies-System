using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class PrintJobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static readonly object _lock = new object();

        public PrintJobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Index() - GET
        public IActionResult Index()
        {
            var jobs = _context.PrintJobs
                .Include(p => p.Student)
                .Include(p => p.Note)
                .Include(p => p.ServerNode)
                .ToList();
            return View(jobs);
        }

        // 2. Create() - GET
        public IActionResult Create()
        {
            ViewBag.Notes = _context.Notes.ToList();
            return View();
        }

        // 3. Create(PrintJob job) - POST
        [HttpPost]
        public IActionResult Create(PrintJob job)
        {
            // FIX: Get current logged-in user instead of using a hardcoded ID
            var email = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return RedirectToAction("Login", "Users");

            lock (_lock)
            {
                job.StudentId = user.Id;
                job.Status = "Queued";
                job.CreatedAt = DateTime.Now;
                
                // FIX: Set mandatory EstimatedPickupTime so the database doesn't reject the save
                job.EstimatedPickupTime = DateTime.Now.AddHours(2);

                _context.PrintJobs.Add(job);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // 4. Cancel(int id) - POST
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var job = _context.PrintJobs.Find(id);

            if (job == null)
                return NotFound();

            // FIX: Prevent cancelling a job that is already finished or already cancelled
            if (job.Status == "Completed" || job.Status == "Cancelled")
            {
                return RedirectToAction("Index"); 
            }

            if (job.Status == "Queued")
            {
                job.Status = "Cancelled";
                job.FineAmount = 0;
            }
            else // If it's "Printing"
            {
                job.Status = "Cancelled";
                job.FineAmount = 100;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 5. UpdateStatus(int id, string newStatus) - POST
        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var job = _context.PrintJobs.Find(id);
            if (job == null) return NotFound();

            job.Status = newStatus;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
