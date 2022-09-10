using System.Collections.Generic;

namespace PycApi.Model
{
    public class ContainersWithCluster
    {
        public ContainersWithCluster(List<Container> containers, double cluster)
        {
            Containers = containers;
            Cluster = cluster;
        }

        public List<Container> Containers { get; set; }
        public double Cluster { get; set; }
    }
}

