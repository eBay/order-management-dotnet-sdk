using System;
using System.Linq;
using System.Windows.Forms;

namespace OrderManagementSample
{
    internal static class Program
    {
        /// <summary>
        /// Opens the order viewer. With <c>--smoke</c>, runs the offline end-to-end check instead
        /// (canned eBay responses, no network, no token) and returns a non-zero exit code on failure.
        /// </summary>
        [STAThread]
        private static int Main(string[] args)
        {
            if (args.Contains("--smoke", StringComparer.OrdinalIgnoreCase))
            {
                return SmokeTest.Run(Console.Out);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            return 0;
        }
    }
}
