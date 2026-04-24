using System;
using System.Collections.Generic;
using System.Text;

namespace PhotocopyProxy
{
    internal class ServerInfo
    {
        public string Host { get; }
        public int Port { get; }
        public int HealthCheckPort { get; }
        public bool IsHealthy { get; set; }

        public double Weight { get; set; } = 1.0;

        // For advanced WRR (smooth algorithm)
        public double CurrentWeight { get; set; } = 0;

        public ServerInfo(string host, int port, int healthCheckPort, double weight = 1.0)
        {
            Host = host;
            Port = port;
            HealthCheckPort = healthCheckPort;
            Weight = weight;
            IsHealthy = true;
        }

        public override string ToString()
        {
            return $"{Host}:{Port} | Healthy: {IsHealthy} | Weight: {Weight}";
        }
    }
}
