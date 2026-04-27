using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PhotocopySystem.Hubs;

namespace PhotocopySystem.Controllers
{
    [Authorize]
    public class PrintJobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<PrintStatusHub> _hub;
        
        // PDC REQUIREMENT: 3 Printers Limit
        private static readonly SemaphoreSlim _printers = new SemaphoreSlim(3, 3);
        private static readonly object _cancelLock = new object();

        public PrintJobsController(
            ApplicationDbContext context,
            IServiceProvider serviceProvider,
            IHubContext<PrintStatusHub> hub)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _hub = hub;
        }

        // 1. Index() - GET (Live fetch from DB)
        public IActionResult Index()
        {
            var jobs = _context.PrintJobs
                .Include(p => p.Student)
                .Include(p => p.Note)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
            return View(jobs);
        }

        // 2. Create() - GET (Bug 8 fix: accept noteId parameter)
        public IActionResult Create(int? noteId)
        {
            ViewBag.Notes = _context.Notes
                .Include(n => n.Subject)
                .Include(n => n.Teacher)
                .ToList();

            var model = new PrintJob { NoteId = noteId };
            return View(model);
        }

        // 3. Create(PrintJob job) - POST (PDC Concurrency with SemaphoreSlim)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrintJob job)
        {
            // Bug 3 fix: ModelState validation
            if (!ModelState.IsValid)
            {
                ViewBag.Notes = _context.Notes
                    .Include(n => n.Subject)
                    .Include(n => n.Teacher)
                    .ToList();
                return View(job);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // 1. Initial Data Setup
            job.StudentId = userId;
            job.CreatedAt = DateTime.Now;
            job.EstimatedPickupTime = DateTime.Now.AddMinutes(30);

            // 2. PDC Concurrency Logic: Try to get a printer
            if (_printers.Wait(0)) // Try to acquire immediately
            {
                job.Status = "Printing";
                _context.PrintJobs.Add(job);
                _context.SaveChanges();

                // Broadcast via SignalR
                await _hub.Clients.All.SendAsync("StatusChanged", job.Id, job.Status, job.StudentId.ToString());

                // 3. Spawning a Background Thread to simulate the printer working
                _ = Task.Run(async () =>
                {
                    await Task.Delay(15000); // Simulate 15 seconds of printing
                    
                    // Update DB when done
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var finishedJob = db.PrintJobs.Find(job.Id);
                        if (finishedJob != null && finishedJob.Status == "Printing")
                        {
                            finishedJob.Status = "Completed";
                            db.SaveChanges();

                            // Broadcast completion via SignalR
                            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<PrintStatusHub>>();
                            await hubContext.Clients.All.SendAsync("StatusChanged", finishedJob.Id, "Completed", finishedJob.StudentId.ToString());
                        }
                    }
                    _printers.Release(); // Free the printer for the next student

                    // Bug 2 fix: Pick up next queued job if one exists
                    await ProcessNextQueuedJobAsync();
                });
            }
            else
            {
                // Bug 1 fix: Use exact "Queued" status (not "Queued (Printers Busy)")
                job.Status = "Queued";
                _context.PrintJobs.Add(job);
                _context.SaveChanges();

                // Broadcast via SignalR
                await _hub.Clients.All.SendAsync("StatusChanged", job.Id, job.Status, job.StudentId.ToString());
            }

            TempData["Success"] = "Print request submitted!";
            return RedirectToAction("Index");
        }

        // 4. UpdateStatus - POST (Using Stored Procedure) — Admin only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStatus(int id, string newStatus)
        {
            // Calling SP 'sp_UpdatePrintStatus'
            _context.Database.ExecuteSqlRaw("EXEC sp_UpdatePrintStatus @JobId = {0}, @NewStatus = {1}", id, newStatus);
            return RedirectToAction("Index");
        }
        
        // 5. Cancel — with lock for concurrency safety (Feature 5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            lock (_cancelLock)
            {
                var job = _context.PrintJobs
                    .FirstOrDefault(j => j.Id == id && j.StudentId == userId);

                if (job == null)
                {
                    TempData["Error"] = "Job not found or not yours.";
                    return RedirectToAction("Index");
                }

                if (job.Status == "Queued")
                {
                    job.Status = "Cancelled";
                    _context.SaveChanges();
                    TempData["Success"] = "Order cancelled. No charge.";
                }
                else if (job.Status == "Printing")
                {
                    job.Status = "Cancelled";
                    _context.SaveChanges();
                    
                    // Force a brand new query completely bypassing the cache to get the trigger result
                    var actualFine = _context.PrintJobs.AsNoTracking()
                                             .Where(j => j.Id == id)
                                             .Select(j => j.FineAmount)
                                             .FirstOrDefault();

                    TempData["Warning"] = $"Cancelled while printing. Fine applied: Rs. {actualFine:F2}";
                }
                else
                {
                    TempData["Error"] = "Cannot cancel a completed or already cancelled job.";
                }
            }

            return RedirectToAction("Index");
        }

        // SP helper for demo purposes (Feature 2)
        private void InsertJobViaSP(int studentId, int? noteId, string documentName, int copies)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC sp_RequestPrint @StudentId={0}, @NoteId={1}, @DocumentName={2}, @Copies={3}",
                studentId, noteId, (object)documentName ?? DBNull.Value, copies
            );
        }

        // Bug 2 fix: Process next queued job when a printer frees up
        private async Task ProcessNextQueuedJobAsync()
        {
            // Try to acquire a printer slot without blocking
            if (!_printers.Wait(0)) return; // No printer free, do nothing

            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var nextJob = db.PrintJobs
                .Where(j => j.Status == "Queued")
                .OrderBy(j => j.CreatedAt)
                .FirstOrDefault();

            if (nextJob == null)
            {
                _printers.Release(); // No job waiting, release the slot back
                return;
            }

            nextJob.Status = "Printing";
            db.SaveChanges();

            // Broadcast via SignalR
            var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<PrintStatusHub>>();
            await hubContext.Clients.All.SendAsync("StatusChanged", nextJob.Id, "Printing", nextJob.StudentId.ToString());

            // Simulate print in background
            await Task.Delay(nextJob.Copies * 3000);

            using var scope2 = _serviceProvider.CreateScope();
            var db2 = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var doneJob = db2.PrintJobs.Find(nextJob.Id);
            if (doneJob != null && doneJob.Status == "Printing")
            {
                doneJob.Status = "Completed";
                db2.SaveChanges();

                var hubContext2 = scope2.ServiceProvider.GetRequiredService<IHubContext<PrintStatusHub>>();
                await hubContext2.Clients.All.SendAsync("StatusChanged", doneJob.Id, "Completed", doneJob.StudentId.ToString());
            }
            _printers.Release();

            // Recursive: keep processing if more jobs are queued
            await ProcessNextQueuedJobAsync();
        }
        // 6. Delete - POST (Task 2)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var job = _context.PrintJobs.Find(id);
            if (job != null)
            {
                _context.PrintJobs.Remove(job);
                _context.SaveChanges();
                TempData["Success"] = "Print request deleted successfully.";
            }
            return RedirectToAction("Index");
        }
    }
}
