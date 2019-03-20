using System;
using System.Windows;

using get_win_net_info;

namespace get_win_net_info_ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                var helper = new NetworkListManagerHelper();
                var infos = helper.GetNetworksInfo();

                if (infos.Length > 0)
                {
                    this.infoTextBox.AppendText($"Detected {infos.Length} network{(infos.Length > 2 ? "s" : "")}:\n");
                    foreach (var info in infos)
                    {
                        this.infoTextBox.AppendText(info.ToString() + "\n");
                    }
                }
                else
                {
                    this.infoTextBox.AppendText("Not connected to any network");
                }

            }
            catch (Exception e)
            {
                this.infoTextBox.AppendText($"Error {e.Message}");
            }
        }
    }
}
