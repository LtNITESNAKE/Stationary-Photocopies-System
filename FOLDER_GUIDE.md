# Project Folder Guide & Team Assignments

This file explains where each member should work and which folders are "Off-Limits."

## 1. Where to Work? (Member Assignments)

| Folder / File | Responsible Member | Task |
| :--- | :--- | :--- |
| **Models/** | Malaika | Create C# classes for Database tables. |
| **Data/** | Malaika | Database Context and Migrations. |
| **Controllers/StudentController.cs** | Muskan | Logic for the Student dashboard. |
| **Controllers/TeacherController.cs** | Fazil | Logic for uploading notes. |
| **Controllers/OperatorController.cs** | Fazil | Logic for managing the print queue. |
| **Controllers/OrderController.cs** | Noor | PDC logic for handling requests & cancellations. |
| **Controllers/StationeryController.cs**| Misbah | Logic for buying items and fines. |
| **Views/Student/** | Muskan | HTML/CSS for the Student portal. |
| **Views/Teacher/** | Fazil | HTML/CSS for the Teacher portal. |
| **Views/Operator/** | Fazil | HTML/CSS for the Operator dashboard. |
| **Views/Stationery/** | Misbah | HTML/CSS for the Stationery shop. |
| **Views/Shared/_Layout.cshtml** | Mujtaba | The main navbar and website frame. |
| **Services/** | Maryam | PDC: The PrintManager and Threading logic. |
| **PhotocopyProxy/** (Project) | Awais & Hadain | The Proxy and Load Balancer console app. |

---

## 2. 🚫 "Do Not Touch" Folders
Do NOT edit or delete these folders. They are managed by the computer:
- `bin/` and `obj/`: Contains temporary compiled files.
- `Properties/`: Contains project settings.
- `wwwroot/lib/`: Contains external libraries like Bootstrap and jQuery.

---

## 3. 🗄️ The Database Question (IMPORTANT)
**Question**: "The database is only on Mujtaba's laptop. How do we all get the data?"

**Answer**: 
You do **not** share the same data rows (like "Student A" or "Order #1"). Instead, you share the **Table Structure**.
1. Malaika will write the code for the tables.
2. She will push her code to GitHub.
3. You will `git pull` her code.
4. You will run `Update-Database` in your own Visual Studio.
5. This creates the **exact same tables** on your own laptop's local SQL Server.
6. Now, you can add your own test data locally to see if your code works!

---

## 🐙 4. Git Rules (What NOT to upload)
I have created a `.gitignore` file for you. It tells GitHub to ignore:
- Large temporary files (`bin/`, `obj/`).
- Your personal Visual Studio settings (`.vs/`).
- User-specific configuration files.

**Always check your branch before you push!**
