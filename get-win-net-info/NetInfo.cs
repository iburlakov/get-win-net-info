using System;
using System.Net;

namespace get_win_net_info
{
    public class NetInfo
    {
        public NetInfo(string name, string dns, string cat, bool isMetered, IPAddress ip)
        {
            this.Name = name ?? string.Empty;
            this.DNS = dns??string.Empty;
            this.Category = cat;
            this.IsMetered = isMetered;
            this.IP = ip;
        }

        public string Name { get; }
        public string DNS { get; }
        public string Category { get; }
        public bool IsMetered { get; }
        public IPAddress IP { get; }

        public override string ToString()
        {
            return string.Format("Name: {0}, DNS: {1}, Category: {2}, IsMetered: {3}, IPv4Address: {4}",
                this.Name,
                this.DNS,
                this.Category,
                this.IsMetered,
                this.IP);
        }
    }
}
