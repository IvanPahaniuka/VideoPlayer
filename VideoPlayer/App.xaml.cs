using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Text;
using ArgsImport;

namespace VideoPlayer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const long ARGS_SIZE = 5_000_000;
        public const string MEMORY_NAME = "VideoPlayer_0x24";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ArgsImporter.Initialize(MEMORY_NAME, ARGS_SIZE);

            if (e.Args.Length > 0)
                ArgsImporter.AddArg(e.Args[0]);

            if (ArgsImporter.IsMainApp == true)
                new MainWindow().Show();
            else
                Shutdown();
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ArgsImporter.Dispose();
        }
    }
}
