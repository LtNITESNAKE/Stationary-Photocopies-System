# Role: Inventory & Business Logic
Name: Misbah Naseer

## Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **Controller**: A C# file that receives button clicks from the website and runs code. When a student clicks "Buy Pen", the request goes to your `StationeryController.cs` and you decide what happens next.
*   **Stock / Inventory**: The number of items available in the shop. If there are 10 pens and someone buys one, the stock becomes 9. Your job is to make sure this number is always correct.
*   **Atomic Update**: A database operation that cannot be interrupted. If two students click "Buy Pen" at the exact same time, an atomic update makes sure the stock goes from 10 to 8 (not 10 to 9 twice). This prevents selling items we don't have.
*   **Fine**: A penalty charge. If a student orders 10 copies and tries to cancel after printing has already started, they must pay a fine. You decide how much.
*   **`_db.SaveChanges()`**: This is the line of code that actually writes your changes to the SQL database. Until you call this, nothing is saved permanently.

## Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "ASP.NET Core MVC CRUD tutorial" (CRUD means Create, Read, Update, Delete. Your "Buy" button is an Update operation because you are updating the stock number).
*   **YouTube Search**: "Bootstrap card grid layout" (Learn how to display stationery items in a nice grid of cards).
*   **W3Schools**: Visit [w3schools.com/html/html_tables.asp](https://www.w3schools.com/html/html_tables.asp) for quick table examples.

---

## Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-misbah-inventory
   ```

---

## Step 2: Step-by-Step Task List

### Phase 1: Stationery Store Page (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer, open the `Views` folder. Right-click `Views` -> `Add` -> `New Folder` -> name it `Stationery`. Right-click the `Stationery` folder -> `Add` -> `View` -> name it `Index.cshtml`.
- **What to do**: Build a page that shows all available items like pens, paper, erasers with their prices and stock.
- **Example Code (Showing items in Bootstrap cards)**:
   ```html
   <div class="container mt-4">
       <h2>Stationery Shop</h2>
       <div class="row">
           <div class="col-md-4 mb-3">
               <div class="card shadow-sm">
                   <div class="card-body">
                       <h5 class="card-title">Blue Pen</h5>
                       <p>Price: 20 PKR</p>
                       <p>In Stock: 15</p>
                       <a href="/Stationery/Buy/1" class="btn btn-primary">Buy Now</a>
                   </div>
               </div>
           </div>
       </div>
   </div>
   ```
- **Next Tasks**:
  - Add more cards for other items (Red Pen, A4 Paper, Eraser, Notebook, etc.).
  - If an item's stock is 0, change the button text to "Out of Stock" and make it grey and unclickable by adding `disabled` to the button tag.
  - If stock is less than 5, add a red text warning below the stock number saying "Low Stock!"

### Phase 2: Buy Logic (Day 3-4)
**Where to build**: In the Solution Explorer, right-click the `Controllers` folder -> `Add` -> `Controller` -> choose "MVC Controller - Empty" -> name it `StationeryController.cs`.
- **What to do**: When a student clicks "Buy Now", your code must check if the item is in stock, then decrease the stock by 1.
- **Example Code (The Buy function)**:
   ```csharp
   public IActionResult Buy(int id) {
       var item = _db.Stationery.Find(id);
       if (item.Stock > 0) {
           item.Stock -= 1;
           _db.SaveChanges();
           return RedirectToAction("Index");
       }
       return BadRequest("This item is out of stock.");
   }
   ```
- **Next Tasks**:
  - Make sure you always check `item.Stock > 0` before decreasing. Never let stock go below zero.
  - After a successful purchase, show a green success message on the page saying "Item purchased successfully!"

### Phase 3: Fine Calculation (Day 5)
**Where to build**: You can create a helper method inside `StationeryController.cs` or create a new file `Services/FineCalculator.cs`.
- **What to do**: Write a function that calculates how much fine a student owes if they cancel a print job late.
- **Task**: The formula is simple: `Fine = Number of Copies * Price Per Page`. For example, if a student ordered 10 copies and the price per page is 2.5 PKR, their fine is 25 PKR.
- **Task**: Work with Noor (the Concurrency specialist) to make sure this fine is saved to the Order record in the database when a late cancellation happens.

---

## Your Daily Git Routine
Every time you write new code:
1. `git add .`
2. `git commit -m "Describe what you changed"`
3. `git push origin dev-misbah-inventory`
4. Go to GitHub and click "Compare & pull request".
