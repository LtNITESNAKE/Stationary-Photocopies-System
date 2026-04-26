using System;
using System.Collections.Generic;

namespace PhotocopyProxy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Photocopy Proxy Server...");

            // Define backend servers
            var servers = new List<ServerInfo>
            {
                new ServerInfo("127.0.0.1", 5001, 6001, 3), // IP address, port, health check port, weight
                new ServerInfo("127.0.0.1", 5002, 6002, 2),
                new ServerInfo("127.0.0.1", 5003, 6003, 1)
            };

            // Create proxy server
            ProxyServer proxy = new ProxyServer(7000, servers);

            // Start proxy (this will run continuously)
            proxy.Start();
        }
    }
}
