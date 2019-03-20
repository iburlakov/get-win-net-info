using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace get_win_net_info
{
    public class NetInfo
    {
        public NetInfo(string name, string cat, bool isMetered, IPAddress ip)
        {
            this.Name = name;
            this.Category = cat;
            this.IsMetered = isMetered;
            this.IP = ip;
        }

        public string Name { get; }
        public string Category { get; }
        public bool IsMetered { get; }
        public IPAddress IP { get; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Category: {1}, IsMetered: {2}, IPv4Address: {3}",
                this.Name,
                this.Category,
                this.IsMetered,
                this.IP);
        }
    }
}
