# Role: Database (DB) Specialist
Name: Malaika Qamar

## 📚 Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **Model / Class**: A C# blueprint that describes what data looks like. For example, a `Student` model will have a Name, ID, and Age. In our project, Models become SQL tables.
*   **Entity Framework Core (EF Core)**: A magic tool provided by Microsoft. Instead of writing complex `CREATE TABLE` SQL queries, you write C# code (Models), and EF Core translates it into SQL tables for you automatically.
*   **Migration**: Think of this as "saving a snapshot" of your C# Models. When you create a Migration, it prepares instructions to build the SQL tables.
*   **Update-Database**: The command that takes the Migration instructions and actually executes them in SQL Server to create the physical tables.

## 🎓 Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "Entity Framework Core Code First for Beginners" (This will show you exactly how to turn C# classes into SQL tables).
*   **Microsoft Docs**: Read the beginner tutorial on [EF Core with ASP.NET Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app).

---

## 💻 Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-malaika-database
   ```

---

## 📋 Step 2: Step-by-Step Task List

### Phase 1: Database Models (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer (right side), right-click the `Models` folder -> click `Add` -> click `Class`. Name the new file `Order.cs`.
- **What to do**: You need to create C# classes that represent the tables in our database.
- **Example Code (Creating the Order model)**:
   ```csharp
   namespace PhotocopySystem.Models;

   public class Order {
       public int Id { get; set; } // This automatically becomes the Primary Key
       public string StudentName { get; set; }
       public string NoteTitle { get; set; }
       public int Copies { get; set; }
       public string Status { get; set; } = "Queued";
       public decimal Fine { get; set; } = 0;
   }
   ```
- **Next Tasks**:
  - Right-click `Models` again and add a class for `User` (with Username, Password, and Role).
  - Add a class for `Note` (with Subject, Title, and Version).
  - Add a class for `StationeryItem` (with Name, Price, and StockQuantity).

### Phase 2: Database Connection (Day 2-3)
**Where to build**: In the Solution Explorer, right-click the project name (PhotocopySystem), click `Add` -> `New Folder`, name it `Data`. Right-click `Data`, click `Add` -> `Class`, name it `ApplicationDbContext.cs`.
- **What to do**: Create a file that tells Entity Framework how to link your Models to the actual SQL Server.
- **Task**: Write the `ApplicationDbContext` class. It must inherit from `DbContext` and contain `DbSet<Order>`, `DbSet<User>`, etc. (Search your learning resources on how to set up a DbContext).

### Phase 3: Migrations (Day 4-5)
**Where to build**: In Visual Studio, go to the top menu: `Tools` -> `NuGet Package Manager` -> `Package Manager Console`. A command window will open at the bottom.
- **What to do**: Turn your C# code into real tables on the `MUJTABA\SQLEXPRESS` server.
- **Task**: In the console, type `Add-Migration InitialCreate` and press Enter.
- **Task**: Then type `Update-Database` and press Enter. If it works, check SQL Server Management Studio—your tables will be there!

---

## 🐙 Your Daily Git Routine
Every time you create a new Model or finish a task:
1. `git add .`
2. `git commit -m "Created the database models and DbContext"`
3. `git push origin dev-malaika-database`
4. Go to GitHub in your web browser and click "Compare & pull request" so Mujtaba can review it.
