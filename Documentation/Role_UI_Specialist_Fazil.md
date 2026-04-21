# Role: UI Specialist (Teacher / Operator)
Name: Muhammad Fazil

## Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **View / .cshtml file**: This is the HTML page that users see in their browser. You will be creating pages for the Teacher (to upload notes) and the Operator (to manage the print queue).
*   **Form with `enctype="multipart/form-data"`**: A special HTML form that allows file uploads. Normal forms only send text. This special form can send files (like PDFs) to the server.
*   **Dashboard**: A control panel page. Think of it like the cockpit of an airplane. The Operator's dashboard shows all active orders, their status, and buttons to control printing.
*   **Auto-Refresh**: Using a tiny piece of Javascript to automatically reload the page every few seconds so the Operator always sees the latest orders without pressing F5.

## Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "HTML file upload form tutorial" (Learn how to create a form that accepts files).
*   **YouTube Search**: "Bootstrap 5 tables and cards" (Learn how to make professional-looking tables and panels).
*   **W3Schools**: Visit [w3schools.com/html/html_forms.asp](https://www.w3schools.com/html/html_forms.asp) for quick form examples.

---

## Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-fazil-admin-ui
   ```

---

## Step 2: Step-by-Step Task List

### Phase 1: Teacher Upload Page (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer, open the `Views` folder. Right-click `Views` -> `Add` -> `New Folder` -> name it `Teacher`. Right-click the `Teacher` folder -> `Add` -> `View` -> name it `Index.cshtml`.
- **What to do**: Create a page where teachers can type a note title, pick a subject, and upload a file.
- **Example Code (File Upload Form)**:
   ```html
   <div class="container mt-4">
       <div class="card p-4 shadow">
           <h3>Upload Notes</h3>
           <form method="post" enctype="multipart/form-data">
               <div class="mb-3">
                   <label>Subject</label>
                   <input type="text" name="Subject" class="form-control" placeholder="e.g. Database Systems">
               </div>
               <div class="mb-3">
                   <label>Note Title</label>
                   <input type="text" name="Title" class="form-control" placeholder="e.g. Chapter 3 Notes">
               </div>
               <div class="mb-3">
                   <label>File</label>
                   <input type="file" name="NoteFile" class="form-control">
               </div>
               <button class="btn btn-primary">Upload Now</button>
           </form>
       </div>
   </div>
   ```
- **Next Tasks**:
  - Add a green alert message that says "Upload Successful!" after the teacher submits the form.
  - Below the form, add a table showing the teacher which notes they have already uploaded (Title, Subject, Date).

### Phase 2: Operator Dashboard (Day 3-4)
**Where to build**: Right-click `Views` -> `Add` -> `New Folder` -> name it `Operator`. Right-click the `Operator` folder -> `Add` -> `View` -> name it `Dashboard.cshtml`.
- **What to do**: Build the "Command Center" where the shop owner sees all student orders and controls printing.
- **Task**: Create a table with columns: Order ID, Student Name, Note Title, Copies, Status, and Action.
- **Task**: In the Action column, add two buttons for each row:
  - A blue button that says "Start Printing" (this will change the status from Queued to Printing).
  - A green button that says "Mark Complete" (this will change the status from Printing to Completed).
- **Task**: Add a search bar at the top so the operator can type a student name or order ID to quickly find a specific order.

### Phase 3: Auto-Refresh (Day 5)
**Where to build**: At the very bottom of `Dashboard.cshtml`, before the closing `</body>` tag (or inside a `@section Scripts {}` block).
- **What to do**: Make the dashboard reload itself every 10 seconds so the operator always sees new orders without pressing F5.
- **Task**: Add this tiny Javascript at the bottom of the Operator Dashboard page:
  ```html
  <script>
      setTimeout(function(){ location.reload(); }, 10000);
  </script>
  ```

---

## Your Daily Git Routine
Every time you finish designing a page:
1. `git add .`
2. `git commit -m "Describe what you changed"`
3. `git push origin dev-fazil-admin-ui`
4. Go to GitHub and click "Compare & pull request".
