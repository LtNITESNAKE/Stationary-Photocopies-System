# Role: Load Balancing Specialist
Name: Hadain Arshad

## Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **Load Balancer**: A traffic police officer for the internet. When 100 students visit the website at the same time, a load balancer splits them: 50 go to Server A, 50 go to Server B. This way, no single server gets overwhelmed.
*   **Round-Robin**: The simplest load balancing strategy. Request 1 goes to Server A. Request 2 goes to Server B. Request 3 goes back to Server A. It just keeps alternating like a circle.
*   **Port**: A number that identifies a specific program running on a computer. Our website runs on Port 5001. We can run a second copy on Port 5002. The Load Balancer switches between them.
*   **Modulo (`%`)**: A math operator that gives you the remainder after division. `0 % 2 = 0`, `1 % 2 = 1`, `2 % 2 = 0`, `3 % 2 = 1`. See the pattern? It keeps alternating between 0 and 1. This is exactly how Round-Robin works.
*   **Health Check**: Before sending a student to a server, you check if that server is actually running. If Server A is crashed, you send everyone to Server B instead.

## Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "What is load balancing explained simply" (Watch a 5-minute animation that shows how load balancers work).
*   **YouTube Search**: "C# modulo operator" (Learn how the `%` symbol works in C#).
*   **Article**: Search Google for "Round Robin load balancing algorithm explained".

---

## Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-hadain-load-balancer
   ```

---

## Step 2: Step-by-Step Task List

### Phase 1: Round-Robin Logic (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer, find the `PhotocopyProxy` project (it is a separate Console App inside the solution). Open `Program.cs` inside that project. You will write your load balancing logic here (or create a new class file inside that project).
- **What to do**: Write a function that alternates between two server addresses every time it is called.
- **Example Code (The Round-Robin counter)**:
   ```csharp
   public class LoadBalancer {
       private static int _requestCount = 0;
       private static string[] _servers = { "http://localhost:5001", "http://localhost:5002" };

       public static string GetNextServer() {
           int index = _requestCount % _servers.Length;
           _requestCount++;
           Console.WriteLine("Routing to: " + _servers[index]);
           return _servers[index];
       }
   }
   ```
- **Next Tasks**:
  - Test your function by calling it 10 times in a loop inside `Main()`. Check the console output. It should print Server A, Server B, Server A, Server B... alternating perfectly.
  - Work with Awais to plug this function into his Proxy Server so real traffic gets routed.

### Phase 2: Integration with Proxy (Day 3-4)
**Where to build**: Inside the `PhotocopyProxy` project, in the same file where Awais is building the TCP Listener.
- **What to do**: Connect your Round-Robin logic to the Proxy.
- **Task**: Every time the Proxy receives a new connection from a student, it should call your `GetNextServer()` function to decide which backend server to forward the request to.
- **Task**: Add a `Console.WriteLine` log message showing "Request #X routed to Server Y" so you can see the balancing in action.

### Phase 3: Testing (Day 5)
**Where to build**: You will need two terminal windows open.
- **What to do**: Prove that the load balancer actually works by running two copies of the website.
- **Task**: In Terminal 1, run the website on port 5001: `dotnet run --urls=http://localhost:5001`
- **Task**: In Terminal 2, run the website on port 5002: `dotnet run --urls=http://localhost:5002`
- **Task**: In Terminal 3, run the Proxy on port 8080. Open a browser and go to `http://localhost:8080`. Check your console logs to see if requests are alternating between 5001 and 5002.

---

## Your Daily Git Routine
Every time you write new code:
1. `git add .`
2. `git commit -m "Describe what you changed"`
3. `git push origin dev-hadain-load-balancer`
4. Go to GitHub and click "Compare & pull request".
