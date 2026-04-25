using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Teacher Role Developer:
        // Teachers use this controller to upload their notes and assignments.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Shows all uploaded notes. Uses .Include() to fetch related Teacher and Subject names.
        public IActionResult Index()
        {
            var notes = _context.Notes
                .Include(n => n.Teacher)
                .Include(n => n.Subject)
                .ToList();
            return View(notes);
        }

        // 2. Create() - GET
        // TODO: Show the upload form. Pass a ViewBag.Subjects so the teacher can select what subject the note is for.

        // 3. Create(Note note) - POST
        // TODO: Save the uploaded Note to the database.
    }
}
