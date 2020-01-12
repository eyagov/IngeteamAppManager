using IngeteamApp.Connectors;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IngeteamApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static DbConnector _dbConnector = new DbConnector();

        public static DbConnector DbConnector
        {
            get { return App._dbConnector; }
            set { App._dbConnector = value; }
        }
    }
}
