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
            
            _context.Notes.Add(note);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}
