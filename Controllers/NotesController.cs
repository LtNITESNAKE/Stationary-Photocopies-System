using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace PhotocopySystem.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Library (Live DB Fetch) — any authenticated user can browse
        public IActionResult Index()
        {
            var notes = _context.Notes
                .Include(n => n.Teacher)
                .Include(n => n.Subject)
                .ToList();

            return View(notes);
        }

        // 2. Upload Note (GET) — Teachers and Admins only
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Create()
        {
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        // 3. Upload Note (POST) — Teachers and Admins only
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Create(Note note)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            note.TeacherId = userId;
            note.UploadedAt = DateTime.Now;
            
            // Task 3: If release time is set, mark as initially locked
            if (note.ReleaseTime.HasValue && note.ReleaseTime.Value > DateTime.Now)
            {
                note.IsLocked = true;
            }

            _context.Notes.Add(note);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        // 4. Delete Note — Admin or the Teacher who uploaded it
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Admin")]
        public IActionResult Delete(int id)
        {
            var note = _context.Notes.Find(id);
            if (note == null) return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            // Security check: only Admin or the actual Teacher who uploaded can delete
            if (!User.IsInRole("Admin") && note.TeacherId != userId)
            {
                return Forbid();
            }

            _context.Notes.Remove(note);
            _context.SaveChanges();
            TempData["Success"] = "Note deleted successfully.";
            
            return RedirectToAction("Index");
        }
    }
}
