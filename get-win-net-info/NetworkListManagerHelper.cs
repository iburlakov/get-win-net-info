using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        private string GetNetworkCaterogyString(NLM_NETWORK_CATEGORY cat)
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
            var dic = new Dictionary<Guid, INetworkConnection>();
            foreach (INetworkConnection con in this.manager.GetNetworkConnections())
            {
                dic[con.GetAdapterId()] = con;
            }

            return dic;
        }

        public NetInfo[] GetNetworksInfo()
        {
            var infos = new List<NetInfo>();

            var networkConnections = this.GetConnections();
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback))
            {
                if (Guid.TryParse(netInterface.Id, out var adapterId))
                {
                    if (networkConnections.TryGetValue(adapterId, out var networkConnection))
                    {
                        var network = networkConnection.GetNetwork();

                        var ip = netInterface.GetIPProperties().UnicastAddresses
                            .FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                            ?.Address ?? new IPAddress(new byte[4]);

                        infos.Add(new NetInfo(network.GetName(),
                            this.GetNetworkCaterogyString(network.GetCategory()),
                            this.IsMetered(networkConnection),
                            ip));
                    }
                }
            }

            return infos.ToArray();
        }
    }
}
