# Role: Proxy & Networking Specialist
Name: Sheikh Muhammad Awais

## Important Vocabulary (What do these words mean?)
Before you start, here are some words you will hear a lot:
*   **Proxy Server**: A middleman program that sits between the user and the real website. The student's browser talks to the Proxy first, and then the Proxy talks to the real website on behalf of the student. The student never directly touches the real server.
*   **TCP (Transmission Control Protocol)**: The language computers use to talk to each other over a network. When you open a website, your browser uses TCP to send and receive data. Your Proxy will use `TcpListener` to listen for these conversations.
*   **Port**: A number that identifies a running program. Think of your computer as an apartment building. Port 8080 is your Proxy's apartment number. Port 5001 is the website's apartment number. The Proxy receives mail at 8080 and delivers it to 5001.
*   **Console Application**: A simple program that runs in a black terminal window (no fancy UI). Your Proxy is a console app because it just listens, forwards, and logs. It does not need buttons or forms.
*   **Socket**: The actual "phone line" connection between two programs. When a student connects to your Proxy, a Socket is created. You read data from it and send data back through it.

## Where to Learn More
If you get stuck, use these resources:
*   **YouTube Search**: "What is a proxy server explained simply" (Watch a 5-minute animation).
*   **YouTube Search**: "C# TcpListener tutorial" (Learn how to make a program listen for connections).
*   **Microsoft Docs**: Read about [TcpListener Class](https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener).

---

## Step 1: Connecting to the Team (Git)
You must download the code Mujtaba started.
1. Open your terminal.
2. Run these commands:
   ```powershell
   git clone https://github.com/LtNITESNAKE/Stationary-Photocopies-System.git
   cd Stationary-Photocopies-System
   git checkout -b dev-awais-proxy
   ```

---

## Step 2: Step-by-Step Task List

### Phase 1: Creating the Proxy Listener (Day 1-2)
**Where to build**: Open Visual Studio. In the Solution Explorer (right side), you will see TWO projects: `PhotocopySystem` (the website) and `PhotocopyProxy` (the console app). Click on `PhotocopyProxy` to expand it. Double-click `Program.cs` inside it. This is where you write ALL your code.
- **What to do**: Make the Proxy listen on Port 8080 and wait for students to connect.
- **Example Code (Starting the Proxy Listener)**:
   ```csharp
   using System.Net;
   using System.Net.Sockets;
   using System.Text;

   Console.WriteLine("=== PhotocopyProxy Server ===");
   Console.WriteLine("Starting proxy on Port 8080...");

   TcpListener listener = new TcpListener(IPAddress.Any, 8080);
   listener.Start();
   Console.WriteLine("Proxy is running. Waiting for connections...");

   while (true) {
       // Wait for a student to connect
       TcpClient client = listener.AcceptTcpClient();
       Console.WriteLine("New connection received at " + DateTime.Now);

       // Read what the student sent
       NetworkStream stream = client.GetStream();
       byte[] buffer = new byte[4096];
       int bytesRead = stream.Read(buffer, 0, buffer.Length);
       string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
       Console.WriteLine("Request: " + request.Split('\n')[0]);

       // TODO: Forward this request to the real website using Hadain's LoadBalancer
       client.Close();
   }
   ```
- **Next Tasks**:
  - Run this code by right-clicking `PhotocopyProxy` in Solution Explorer -> `Set as Startup Project` -> then press the green Play button. You should see "Proxy is running" in the console.
  - Open your web browser and go to `http://localhost:8080`. You should see "New connection received" appear in your console. That means it is working!

### Phase 2: Forwarding Traffic (Day 3-4)
**Where to build**: Inside the same `Program.cs` file in the `PhotocopyProxy` project, in the `// TODO` section.
- **What to do**: After reading the student's request, you must send it to the real website and return the website's response back to the student.
- **Task**: Use Hadain's `LoadBalancer.GetNextServer()` function to get the correct server URL (either Port 5001 or 5002).
- **Task**: Create a new `TcpClient` connection to that server, send the student's request bytes to it, read the response bytes, and send them back to the original student's connection.
- **Task**: Add a log line: `Console.WriteLine("Forwarded to: " + serverUrl)` so you can see in the console which server handled each request.

### Phase 3: Final Integration (Day 5)
**Where to build**: You will need multiple terminal windows.
- **What to do**: Run the entire distributed system at once to prove it works.
- **Task**: Terminal 1 - Run the website on port 5001.
- **Task**: Terminal 2 - Run the website on port 5002.
- **Task**: Terminal 3 - Run your Proxy on port 8080.
- **Task**: Open a browser and go to `http://localhost:8080`. Watch your Proxy console. It should show requests being forwarded to 5001 and 5002 alternately.

---

## Your Daily Git Routine
Every time you write new code:
1. `git add .`
2. `git commit -m "Describe what you changed"`
3. `git push origin dev-awais-proxy`
4. Go to GitHub and click "Compare & pull request".
