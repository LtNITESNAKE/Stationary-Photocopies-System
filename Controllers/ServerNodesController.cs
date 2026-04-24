using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotocopySystem.Data;
using PhotocopySystem.Models;
using System.Linq;

namespace PhotocopySystem.Controllers
{
    public class ServerNodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServerNodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // TODO for Awais & Hadain: You are managing the simulated Physical Printers (ServerNodes).
        // The Photocopier Operator uses these nodes to distribute Print Jobs to different machines!

        // 1. Index() - GET (COMPLETED EXAMPLE)
        // What it does: Shows a dashboard of all Server Nodes and their CurrentActiveJobs.
        public IActionResult Index()
        {
            var servers = _context.ServerNodes.ToList();
            return View(servers);
        }

        // 2. Create() - GET
        // TODO: Show the form to add a new physical Server Node to the load balancer.

        // 3. Create(ServerNode node) - POST
        // TODO: Save the ServerNode to the database.
    }
}
