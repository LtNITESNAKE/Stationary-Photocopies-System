//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PhotocopySystem.Data;
//using PhotocopySystem.Models;
//using System.Linq;

//namespace PhotocopySystem.Controllers
//{
//    public class ReviewsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public ReviewsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // TODO for Fazil: You are building the feedback system! 
//        // Let Students leave reviews on the uploaded Notes or the Stationeries they bought.

//        // 1. Index() - GET (COMPLETED EXAMPLE)
//        // What it does: Shows all reviews, including the Student's name.
//        public IActionResult Index()
//        {
//            // TODO: Once you finish the Review Model, uncomment the .Include() lines below!
//            var reviews = _context.Reviews
//                /*.Include(r => r.User)*/ // The Student
//                /*.Include(r => r.Product)*/ // The Item
//                .ToList();
//            return View(reviews);
//        }

//        // 2. Create() - GET
//        // TODO: Show the review form.

//        // 3. Create(Review review) - POST
//        // TODO: Save the review. Use the logged-in Student's UserId!
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        
        public IActionResult Index()
        {
            var reviews = _context.Reviews
                .Include(r => r.User)       
                .Include(r => r.Product)   
                .ToList();
            return View(reviews);
        }

        
        public IActionResult Create()
        {
            ViewBag.ProductId = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Review review)
        {
            review.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            
            ViewBag.ProductId = new SelectList(_context.Products, "Id", "Name", review.ProductId);
            return View(review);
        }

        
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var review = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefault(m => m.Id == id);

            if (review == null) return NotFound();

            return View(review);
        }

        
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var review = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefault(m => m.Id == id);

            if (review == null) return NotFound();

            return View(review);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}