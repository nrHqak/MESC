using System;
using System.IO;
using System.Windows.Forms;
using MESC.Forms;

namespace MESC
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);

            Application.Run(new LoginForm());
        }
    }
}
