using System;
using System.Windows.Forms;

namespace Lengko.Datatables.Demo.CSharp;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
