using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DataProcess
{
    /// <summary>
    /// Interaction logic for App.xaml
    
    public partial class App : Application
    {
        System.Threading.Mutex mutex;

        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "DataProcess", out ret);
            if (!ret)
            {
                MessageBox.Show("程序只能运行一次!");
                Environment.Exit(0);
            }
        }
    }
}
