using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

using NETWORKLIST;

namespace get_win_net_info
{
    public class NetworkListManagerHelper
    {
        private readonly NetworkListManagerClass manager;
        public NetworkListManagerHelper()
        {
            this.manager = new NetworkListManagerClass();
        }

        private string GetNetworkCategoryString(NLM_NETWORK_CATEGORY cat)
        {
           return cat.ToString().Substring(nameof(NLM_NETWORK_CATEGORY).Length+1);
        }

        private bool IsMetered(INetworkConnection connection)
        {
            if (connection is INetworkConnectionCost networkConnectionCost)
            {
                networkConnectionCost.GetCost(out uint netCost);

                return ((NLM_CONNECTION_COST) netCost).HasFlag(NLM_CONNECTION_COST.NLM_CONNECTION_COST_FIXED);
            }

            return false;
        }

        private Dictionary<Guid, INetworkConnection> GetConnections()
        {
            var dict = new Dictionary<Guid, INetworkConnection>();
            foreach (INetworkConnection connection in this.manager.GetNetworkConnections())
            {
                dict[connection.GetAdapterId()] = connection;
            }

            return dict;
        }

        public NetInfo[] GetNetworksInfo()
        {
            var infos = new List<NetInfo>();

            var networkConnections = this.GetConnections();
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback))
            {
                if (Guid.TryParse(networkInterface.Id, out var adapterId))
                {
                    if (networkConnections.TryGetValue(adapterId, out var networkConnection))
                    {
                        var network = networkConnection.GetNetwork();

                        var dns = networkInterface.GetIPProperties().DnsSuffix;
                        var ip = networkInterface.GetIPProperties().UnicastAddresses
                            .FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                            ?.Address ?? new IPAddress(new byte[4]);

                        infos.Add(
                            new NetInfo(
                                network.GetName(),
                                dns,
                                this.GetNetworkCategoryString(network.GetCategory()),
                                this.IsMetered(networkConnection),
                                ip));
                    }
                }
            }

            return infos.ToArray();
        }
    }
}
