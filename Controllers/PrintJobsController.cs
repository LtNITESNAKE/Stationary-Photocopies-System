using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
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

        // TODO for Maryam (Threading & Concurrency Task!):
        // You are managing the Core PDC Print Queue! 
        // Flow: Students request copies of Notes -> Threads handle concurrency -> Operator manages status -> Fines apply if cancelled late.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Shows the master queue of all jobs to process, including Student name and Note title.
        public IActionResult Index()
        {
            // TODO: Once you finish adding the Foreign Keys to PrintJob.cs, uncomment the .Include() lines!
            var jobs = _context.PrintJobs
        .Include(p => p.Student)
        .Include(p => p.Note)
        .Include(p => p.ServerNode)
        .ToList();
            return View(jobs);
        }

        // 2. Create() - GET
        // TODO: Show a form for a STUDENT to select a NoteId (from Notes table) and request Copies.
        public IActionResult Create()
        {
            ViewBag.Notes = _context.Notes.ToList();
            return View();
        }

        // 3. Create(PrintJob job) - POST
        // TODO (PDC THREADING TASK): When a student confirms printing, handle concurrency using threads.
        // Use locks/monitors safely to update the PrintJobs table and assign to ServerNodes.
        [HttpPost]
        public IActionResult Create(PrintJob job)
        {
            lock (_lock)
            {
                job.Status = "Queued";
                job.CreatedAt = DateTime.Now;

                _context.PrintJobs.Add(job);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        // 4. Cancel(int id) - POST
        // TODO: A Student attempts to cancel an order. 
        // If status is 'Queued', cancel for free. If 'Printing' or 'Completed', apply FineAmount.
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var job = _context.PrintJobs.Find(id);

            if (job == null)
                return NotFound();

            if (job.Status == "Queued")
            {
                job.Status = "Cancelled";
                job.FineAmount = 0;
            }
            else
            {
                job.Status = "Cancelled";
                job.FineAmount = 100;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 5. UpdateStatus(int id, string newStatus) - POST
        // TODO: Operator uses this to move jobs from Queued -> Printing -> Completed.
        [HttpPost]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            var job = _context.PrintJobs.Find(id);

            if (job == null)
                return NotFound();

            job.Status = newStatus;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
