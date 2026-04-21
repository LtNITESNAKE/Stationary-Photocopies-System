# Role: Lead Architect & Integration
Name: Muhammad Mujtaba

## 📚 Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **MVC (Model-View-Controller)**: The way we organize our code. **Models** are data (Database), **Views** are the user interface (HTML), and **Controllers** are the brain (C# logic).
*   **Layout / `_Layout.cshtml`**: The master template of the website. It contains the top navigation bar and the bottom footer. Every other page loads *inside* this layout.
*   **Connection String**: A secret password and address that tells our code exactly how to find and talk to your SQL Server (`MUJTABA\SQLEXPRESS`).
*   **Pull Request (PR)**: When a teammate finishes code on their laptop, they ask you to "pull" it into the main project. You must review it before saying yes.

## 🎓 Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "ASP.NET Core MVC for Beginners" (Watch a 30-minute crash course to understand how folders link together).
*   **Microsoft Docs**: Read about [Views and Layouts in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/layout).
*   **GitHub**: Search YouTube for "How to review and merge Pull Requests in GitHub".

---

## 💻 Step 1: Connecting to the Team (Git)
You are the owner, so you must initialize the project first.
1. Open your terminal (PowerShell or Command Prompt).
2. Type these exact commands one by one to set up the connection between your computer and GitHub:
   ```powershell
   mkdir PhotocopySystem
   cd PhotocopySystem
   dotnet new mvc
   git init
   git add .
   git commit -m "Initial project setup"
   git branch -M main
   git remote add origin https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   git push -u origin main
   ```

---

## 📋 Step 2: Step-by-Step Task List

### Phase 1: Layout & Styling (Day 1)
**Where to build**: Open Visual Studio. On the right side, look at the **Solution Explorer**. Open the `Views` folder -> open the `Shared` folder -> double-click `_Layout.cshtml`.
- **What to do**: You need to update the main navigation bar so users can click links to go to the Student, Teacher, and Operator pages.
- **Example Code (Put this inside the `<ul class="navbar-nav flex-grow-1">` tag)**:
  ```html
  <li class="nav-item">
      <a class="nav-link text-dark" asp-controller="Student" asp-action="Index">Student Portal</a>
  </li>
  ```
- **Next Tasks**:
  - Add similar `<li class="nav-item">` links for Teacher, Operator, and Stationery portals.
  - Scroll to the bottom of the file and update the `<footer>` tag to include the names of your 9 group members.

### Phase 2: Database Setup (Day 2)
**Where to build**: In the Solution Explorer, double-click the file named `appsettings.json`.
- **What to do**: You must tell the application where the SQL Server is located on your laptop.
- **Task**: Add a connection string that points to `MUJTABA\SQLEXPRESS`. Ensure the database name is set to `PhotocopyDB`.

### Phase 3: Integration & Review (Day 3-5)
**Where to build**: Your web browser (go to your GitHub repository page).
- **What to do**: You are the gatekeeper. No broken code should enter the `main` branch.
- **Task**: Log into GitHub daily. Click on the "Pull Requests" tab. Check the code submitted by your teammates. If it looks correct, click "Merge". If it has errors, leave a comment telling them to fix it.
- **Task**: Help Awais and Hadain ensure their Proxy Server program is running and can send traffic to your MVC website.

---

## 🐙 Your Daily Git Routine
Every time you change a file on your laptop, save it, open Terminal, and type:
1. `git add .`
2. `git commit -m "Describe what you changed"`
3. `git push origin main`
