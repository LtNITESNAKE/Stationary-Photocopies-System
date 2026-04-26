using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PhotocopyProxy
{
    internal class ProxyServer
    {
        private readonly int _port;   //proxy server port
        private readonly WeightedLoadBalancer _loadBalancer;  // Weighted Load Balancer instance
        private readonly List<ServerInfo> _servers;    // List of backend servers

        public ProxyServer(int port, List<ServerInfo> servers)
        {
            _port = port;
            _servers = servers;
            _loadBalancer = new WeightedLoadBalancer(_servers);
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();

            Console.WriteLine($"Proxy Server started on port {_port}");

            // Start health check in background
            _ = StartHealthCheck();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            NetworkStream clientStream = client.GetStream();

            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead == 0)
                {
                    client.Close();
                    return;
                }

                byte[] requestData = new byte[bytesRead]; 
                Array.Copy(buffer, requestData, bytesRead); 

                Console.WriteLine("Received request from client");

                await ProcessRequest(clientStream, requestData);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client handling error: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        private async Task ProcessRequest(NetworkStream clientStream, byte[] data)
        {
            int attempts = 0;
            int maxRetries = _servers.Count; 

            while (attempts < maxRetries) // try each server at most once
            {
                ServerInfo server = null;

                try
                {
                    server = _loadBalancer.GetNextServer();
                    if(server == null)
                    {
                        throw new Exception("No healthy servers available");
                    }

                    Console.WriteLine($"Forwarding to {server}");

                    using (TcpClient backend = new TcpClient())
                    {
                        await backend.ConnectAsync(server.Host, server.Port);
                        NetworkStream backendStream = backend.GetStream();

                        // Send request to backend
                        await backendStream.WriteAsync(data, 0, data.Length);

                        // Receive response and stream it fully back to the client
                        // FIX: We use CopyToAsync here so it reads until the backend closes the stream, supporting websites > 4KB!
                        await backendStream.CopyToAsync(clientStream);

                        return; // success
                    }
                }
                catch
                {
                    if (server != null)
                    {
                        Console.WriteLine($"Server {server.Port} FAILED");
                        server.isHealthy = false;
                    }

                    attempts++;
                }
            }

            // If all retries failed
            string errorMsg = "All servers are down!";
            byte[] errorData = Encoding.UTF8.GetBytes(errorMsg);
            await clientStream.WriteAsync(errorData, 0, errorData.Length);
        }

        // 🔄 Background Health Check
        private async Task StartHealthCheck()
        {
            while (true)
            {
                foreach (var s in _servers)
                {
                    try
                    {
                        using (TcpClient client = new TcpClient())
                        {
                            await client.ConnectAsync(s.Host, s.healthCheckPort);

                            if (!s.isHealthy)
                                Console.WriteLine($"Server {s.Port} RECOVERED");

                            s.isHealthy = true;
                        }
                    }
                    catch
                    {
                        if (s.isHealthy)
                            Console.WriteLine($"Server {s.Port} DOWN");

                        s.isHealthy = false;
                    }
                }

                await Task.Delay(5000); // every 5 seconds
            }
        }
    }
}