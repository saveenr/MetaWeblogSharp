// ----------------------
//  Tests - Program.cs 
//  mmachado - 9/15/2013 
// ----------------------

using System;

using System.Linq;

using System.Windows.Forms;

namespace Tests
{
    static class Program
    {
        public static void Run()
        {
            var test = new MetaWeblogSharpSample1();

            test.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Run();

            return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}