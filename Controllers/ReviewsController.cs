using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Fazil: You are building the feedback system! 
        // Let Students leave reviews on the uploaded Notes or the Stationeries they bought.

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Shows all reviews, including the Student's name.
        public IActionResult Index()
        {
            // TODO: Once you finish the Review Model, uncomment the .Include() lines below!
            var reviews = _context.Reviews
                /*.Include(r => r.User)*/ // The Student
                /*.Include(r => r.Product)*/ // The Item
                .ToList();
            return View(reviews);
        }

        // 2. Create() - GET
        // TODO: Show the review form.

        // 3. Create(Review review) - POST
        // TODO: Save the review. Use the logged-in Student's UserId!
    }
}
