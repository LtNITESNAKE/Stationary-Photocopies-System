# Role: PDC Specialist (Concurrency)
Name: Noor-ul-huda

## Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **Async / Await**: These two keywords tell the server "don't wait for this task to finish before helping the next student." Without them, if Student A clicks Order, Student B has to wait until A's order is fully saved before the server even looks at B.
*   **Controller**: A C# file that receives requests from the website (like clicking a button) and decides what to do (save to database, show a page, etc.). Every button click on the website eventually reaches a Controller.
*   **Transaction**: A way to group database actions together. If you are changing two things at the same time (like removing an order AND adding a fine), a Transaction makes sure BOTH happen or NEITHER happens. This prevents half-broken data.
*   **Concurrency**: Many things happening at the same time. In our project, 10 students might click "Order" at the exact same second. Your job is to make sure the server handles all 10 without crashing.

## Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "C# async await explained simply" (Watch a short video to understand why we use async).
*   **YouTube Search**: "ASP.NET Core Controller tutorial" (Learn how buttons on HTML connect to C# code).
*   **Microsoft Docs**: Read about [Async programming in C#](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/).

---

## Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-noor-concurrency
   ```

---

## Step 2: Step-by-Step Task List

### Phase 1: Handling Many Requests (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer (right side), right-click the `Controllers` folder -> click `Add` -> click `Controller` -> choose "MVC Controller - Empty" -> name it `OrderController.cs`.
- **What to do**: When a student clicks "Order Now" on the website, the request comes here. You must save the order to the database without blocking other students.
- **Example Code (Using Async to handle multiple students at once)**:
   ```csharp
   namespace PhotocopySystem.Controllers;
   using Microsoft.AspNetCore.Mvc;
   using PhotocopySystem.Models;

   public class OrderController : Controller {
       private readonly ApplicationDbContext _db;

       public OrderController(ApplicationDbContext db) {
           _db = db;
       }

       [HttpPost]
       public async Task<IActionResult> Create(Order newOrder) {
           newOrder.Status = "Queued";
           await _db.Orders.AddAsync(newOrder);
           await _db.SaveChangesAsync();
           return RedirectToAction("MyOrders");
       }
   }
   ```
- **Next Tasks**:
  - Add a `MyOrders` action method that fetches all orders for the logged-in student and returns a View showing their order status.
  - Add an `Index` action that shows available notes to order from.

### Phase 2: Cancellation Logic (Day 3-4)
**Where to build**: Inside the same `OrderController.cs` file.
- **What to do**: A student should be able to cancel their order, but ONLY if printing has not started yet.
- **Task**: Create a method called `Cancel` that takes an order ID. First, find the order in the database using `_db.Orders.Find(id)`. Then check its `Status` field.
  - If the Status is "Queued": Delete the order from the database and redirect the student to a success page.
  - If the Status is "Printing" or "Completed": Do NOT delete. Instead, calculate a fine (work with Misbah on the formula) and save the fine amount to the order. Show the student a message saying they must pay.

### Phase 3: Estimated Pickup Time (Day 5)
**Where to build**: Inside the `MyOrders` View or the `OrderController`.
- **What to do**: Show the student roughly when their print will be ready.
- **Task**: Count how many orders are ahead of this student in the queue. Multiply that number by 5 (since each print takes about 5 seconds in our simulation). Display a message like "Your order will be ready in approximately X seconds."

---

## Your Daily Git Routine
Every time you write new code:
1. `git add .`
2. `git commit -m "Describe what you changed"`
3. `git push origin dev-noor-concurrency`
4. Go to GitHub and click "Compare & pull request".
