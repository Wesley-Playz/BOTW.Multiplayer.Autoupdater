using System;
using System.Windows.Forms;

namespace BotWMultiplayerUpdaterGUI
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); // Per-monitor DPI awareness
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // Ensure Form1 is referenced here
        }
    }
}
