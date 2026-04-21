# Role: PDC Specialist (Print Engine)
Name: Maryam Munir

## 📚 Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **Thread**: A single worker inside the computer program. If 10 students order at once, 10 "threads" (workers) are created to handle them.
*   **Race Condition**: A major problem in PDC. If two threads try to use the photocopy machine at the exact same millisecond, the system crashes or prints garbage.
*   **Lock / Synchronization**: A mechanism that acts like a key to a room. When Thread A takes the "lock", it goes into the photocopy room and locks the door. Thread B must wait outside until Thread A finishes and unlocks the door.
*   **Queue (FIFO)**: A First-In-First-Out line. Like a line at a grocery store. The first order added to the queue is the first one to be printed.

## 🎓 Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "C# Threading and Locks tutorial" (Watch how `lock (object)` works to prevent race conditions).
*   **Microsoft Docs**: Read about [lock statement (C# reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock).

---

## 💻 Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-maryam-pdc-engine
   ```

---

## 📋 Step 2: Step-by-Step Task List

### Phase 1: Thread Synchronization (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer (right side), right-click the project name, click `Add` -> `New Folder`, name it `Services`. Right-click `Services`, click `Add` -> `Class`, name it `PrintJobManager.cs`.
- **What to do**: You must ensure that only one print job happens at a time to avoid errors (race conditions).
- **Example Code (Using a lock to protect the printer)**:
   ```csharp
   namespace PhotocopySystem.Services;
   using System.Threading;

   public class PrintJobManager {
       // The 'key' to the photocopy room
       private static readonly object _printLock = new object();
       
       public void ExecutePrintJob(int orderId) {
           lock (_printLock) {
               // Only one thread can enter this block at a time
               Console.WriteLine("Printing Order: " + orderId);
               Thread.Sleep(5000); // Simulate printing taking 5 seconds
               Console.WriteLine("Finished Order: " + orderId);
           }
       }
   }
   ```
- **Next Tasks**:
  - Inside `PrintJobManager`, create a `Queue<int>` to store order IDs that are waiting to be printed.
  - Implement a background loop that constantly checks if the Queue has items (`Queue.Count > 0`). If it does, Dequeue the ID and run `ExecutePrintJob(id)`.

### Phase 2: Status Tracking (Day 3-4)
**Where to build**: Inside the same `ExecutePrintJob` method in `PrintJobManager.cs`.
- **What to do**: You need to update the database as the printing progresses so the UI team can show progress bars to the student.
- **Task**: When the lock is entered, write code to find the order in the database (ask Malaika how to access the DB) and change its status to "Printing."
- **Task**: When the 5 seconds (`Thread.Sleep`) are finished, update the status to "Completed".

### Phase 3: Stress Testing (Day 5)
**Where to build**: Inside a Controller (ask Noor for help).
- **What to do**: You must verify that your `lock` works when many people use it.
- **Task**: Write a simple test where you trigger the print job 10 times instantly in a `for` loop. Watch the console output. If the console shows "Printing Order 1" then waits 5 seconds, then "Finished", then starts "Printing Order 2", your lock is working perfectly!

---

## 🐙 Your Daily Git Routine
Every time you write new code:
1. `git add .`
2. `git commit -m "Added lock mechanism for printing"`
3. `git push origin dev-maryam-pdc-engine`
4. Create a Pull Request on GitHub.
