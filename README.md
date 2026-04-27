# 🖨️ The Photocopy Portal

![.NET Core](https://img.shields.io/badge/.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![SignalR](https://img.shields.io/badge/SignalR-0078D4?style=for-the-badge&logo=microsoft&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)

> **Live Demo:** [http://ThePhotocopyPortal.somee.com](http://ThePhotocopyPortal.somee.com)

The **Photocopy Portal** is an advanced, role-based university stationery and printing management system. Developed as a capstone project for **Parallel and Distributed Computing (PDC)**, this application demonstrates robust backend concurrency control, real-time client broadcasting, and database-level data integrity.

---

## ✨ Key Features

### 🏢 Role-Based Access Control
- **Admin**: Full system control. Manage users, monitor all orders, update inventory, and manually adjust print queues.
- **Teacher**: Dedicated library access to upload course notes and assignments for students to access seamlessly.
- **Student**: Place stationery orders, request concurrent print jobs, and monitor live status queues.

### ⚡ Parallel & Distributed Computing (PDC)
- **Bounded Resource Pooling**: Uses `SemaphoreSlim(3, 3)` to model physical printer limitations. The system successfully handles heavy concurrent web traffic while restricting the active printing threads to 3 parallel consumers.
- **Optimistic Concurrency**: Implements `RowVersion` timestamps in the SQL database to prevent race conditions during high-volume inventory updates.
- **Asynchronous Non-Blocking I/O**: Heavy use of `async/await` ensures the web server thread pool remains responsive during intense database operations.

### 📡 Real-Time Broadcasting
- Integrated **SignalR (WebSockets)** to instantly push print job status updates (Queued ➔ Printing ➔ Completed) to all connected clients without requiring page refreshes.

### 🗄️ Advanced Database Architecture
- **Stored Procedures**: Encapsulated business logic for robust print job queuing.
- **SQL Triggers**: 
  - `trg_AutoDeductStock`: Automatically balances inventory upon order placement.
  - `trg_ApplyCancellationFine`: Mathematically ensures fines are applied if a student cancels a job that has already begun parallel processing.

---

## 🛠️ Technology Stack

- **Frontend:** HTML5, CSS3, Bootstrap 5, Vanilla JavaScript
- **Backend:** ASP.NET Core MVC (C#)
- **Database:** Microsoft SQL Server (T-SQL)
- **ORM:** Entity Framework Core
- **Real-Time Web:** Microsoft SignalR
- **Hosting:** Somee.com (Cloud IIS & SQL Server)

---

## 🚀 Running Locally

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (or newer)
- Microsoft SQL Server (LocalDB or Express)

### Setup Instructions
1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/PhotocopySystem.git
   cd PhotocopySystem
   ```
2. **Setup the Database**
   Open SQL Server Management Studio (SSMS) and execute the `PhotocopySystem_Complete_DB.sql` script to build the schema, triggers, and seed data.
3. **Configure Connection String**
   Update `appsettings.json` with your local SQL Server instance.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=PhotocopySystemDB;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```
4. **Run the Application**
   ```bash
   dotnet run
   ```



---


---
*Developed for University Semester 6 - Parallel and Distributed Computing (PDC).*
