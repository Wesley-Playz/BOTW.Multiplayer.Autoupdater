using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BotWMultiplayerUpdaterGUI
{
    static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

        [STAThread]
        static void Main()
        {
            SetProcessDpiAwarenessContext(-4); // -4 corresponds to Per-Monitor V2 DPI awareness
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); // for .NET Core / .NET 5+ projects
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
