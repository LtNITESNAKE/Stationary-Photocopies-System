using PhotocopyProxy;

internal class WeightedLoadBalancer
{
    private readonly List<ServerInfo> _servers;

    public WeightedLoadBalancer(List<ServerInfo> servers)
    {
        _servers = servers;
    }

    public ServerInfo GetNextServer()
    {
        ServerInfo best = null;
        double totalWeight = 0;

        foreach (var s in _servers)
        {
            if (!s.isHealthy) continue;

            totalWeight += s.weight;

            // Add weight each round
            s.CurrentWeight += s.weight;

            if (best == null || s.CurrentWeight > best.CurrentWeight)
            {
                best = s;
            }
        }

        if (best == null)
        {
            throw new Exception("No healthy servers available.");
        }

        // Reduce selected server weight
        best.CurrentWeight -= totalWeight;

        return best;
    }
}