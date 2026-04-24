using System;
using System.Collections.Generic;
using System.Text;

namespace PhotocopyProxy
{
    internal class ServerInfo
    {
        public string Host { get; }
        public int Port { get; }
        public int healthCheckPort { get; }
        public bool isHealthy { get; set; }
        public double weight { get; set; } = 1.0; // Default weight for load balancing
        public ServerInfo(string host, int port, int healthCheckPort)
        {
            Host = host;
            Port = port;
            this.healthCheckPort = healthCheckPort;
            isHealthy = true; // Assume server is healthy at initialization
        }

        public ServerInfo(string host, int port, int healthCheckPort, double weight)
        {
            Host = host;
            Port = port;
            this.healthCheckPort = healthCheckPort;
            isHealthy = true; // Assume server is healthy at initialization
            this.weight = weight;
        }

        public override string ToString()
        {
            return $"{Host}:{Port} (Health Check Port: {healthCheckPort}, Healthy: {isHealthy}, Weight: {weight})";
        }

    }
}
