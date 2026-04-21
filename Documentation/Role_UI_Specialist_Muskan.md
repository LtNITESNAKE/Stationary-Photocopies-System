# Role: UI Specialist (Student Portal)
Name: Muskan

## Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **View / .cshtml file**: This is the HTML page that users see in their browser. In our project, every page (Login, Dashboard, Order Form) is a `.cshtml` file stored in the `Views` folder. It is basically HTML with the ability to show C# data.
*   **Bootstrap**: A free CSS toolkit from Twitter. Instead of writing your own CSS from scratch, you use pre-made classes like `btn-primary` (blue button), `table-hover` (table that highlights when you hover), and `card` (a nice box with shadow). It is already included in our project.
*   **Form / `<form>` tag**: An HTML element that collects input from the user (like their name, number of copies) and sends it to the server when they click "Submit."
*   **Badge**: A small colored label. We use badges to show order status: yellow badge for "Queued", blue for "Printing", green for "Ready."

## Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "HTML forms for beginners" (Learn how `<input>`, `<select>`, and `<button>` work).
*   **YouTube Search**: "Bootstrap 5 crash course" (Learn all the classes like `container`, `row`, `col`, `card`, `btn`).
*   **W3Schools**: Visit [w3schools.com/bootstrap5](https://www.w3schools.com/bootstrap5/) for quick examples you can copy.

---

## Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-muskan-student-ui
   ```

---

## Step 2: Step-by-Step Task List

### Phase 1: Student Dashboard (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer, open the `Views` folder. Right-click `Views` -> `Add` -> `New Folder` -> name it `Student`. Right-click the `Student` folder -> `Add` -> `View` -> name it `Index.cshtml`.
- **What to do**: Build the main page students see when they log in. It should show a table of all available notes.
- **Example Code (A clean Bootstrap Table)**:
   ```html
   <div class="container mt-4">
       <h2>Available Notes</h2>
       <table class="table table-hover shadow-sm">
           <thead class="table-dark">
               <tr>
                   <th>Subject</th>
                   <th>Teacher</th>
                   <th>Version</th>
                   <th>Action</th>
               </tr>
           </thead>
           <tbody>
               <tr>
                   <td>Database Systems</td>
                   <td>Dr. Ahmed</td>
                   <td>v1</td>
                   <td><a href="/Order/Create" class="btn btn-sm btn-primary">Order Now</a></td>
               </tr>
           </tbody>
       </table>
   </div>
   ```
- **Next Tasks**:
  - Add more rows to the table (these will later come from the database, but for now you can type them manually to see how the page looks).
  - Add a search bar above the table using an `<input type="text">` field so students can filter notes by subject.

### Phase 2: Order Form (Day 3-4)
**Where to build**: Right-click the `Views/Student` folder -> `Add` -> `View` -> name it `Order.cshtml`.
- **What to do**: Create a form where the student types how many copies they want and picks a version.
- **Task**: Add an `<input type="number">` for the number of copies. Set `min="1"` so they cannot type 0.
- **Task**: Add a `<select>` dropdown with options like "Version 1", "Version 2."
- **Task**: Add a green `<button class="btn btn-success">Confirm Order</button>` at the bottom.
- **Task (Optional but impressive)**: Add a small Javascript snippet that multiplies the number of copies by a price (e.g., 5 PKR) and displays the total in real-time as the student types.

### Phase 3: Order Tracking (Day 5)
**Where to build**: Right-click the `Views/Student` folder -> `Add` -> `View` -> name it `MyOrders.cshtml`.
- **What to do**: Show the student a list of their own orders with colored status badges.
- **Task**: Create a table with columns for "Note Title", "Copies", and "Status."
- **Task**: For the Status column, use Bootstrap badges:
  - `<span class="badge bg-warning text-dark">Queued</span>` (Yellow)
  - `<span class="badge bg-info">Printing</span>` (Blue)
  - `<span class="badge bg-success">Ready for Pickup</span>` (Green)

---

## Your Daily Git Routine
Every time you finish designing a page:
1. `git add .`
2. `git commit -m "Describe what you changed"`
3. `git push origin dev-muskan-student-ui`
4. Go to GitHub and click "Compare & pull request".
