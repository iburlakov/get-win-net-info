using System;
using System.Text;

using get_win_net_info;

namespace get_win_net_info_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var helper = new NetworkListManagerHelper();
                var infos = helper.GetNetworksInfo();

                if (infos.Length > 0)
                {
                    var builder = new StringBuilder($"Detected {infos.Length} network{(infos.Length > 2 ? "s" : "")}:\n");
                    foreach (var info in infos)
                    {
                        builder.AppendLine(info.ToString());
                    }

                    Console.WriteLine(builder);
                }
                else
                {
                    Console.WriteLine("Not connected to any network");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            Console.ReadKey();
        }
    }
}
