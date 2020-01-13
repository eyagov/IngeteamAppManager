using IngeteamApp.VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IngeteamApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProgrammersManager_Click(object sender, RoutedEventArgs e)
        {
            ProgrammersViewer pv = new ProgrammersViewer();
            pv.ShowDialog();
        }
        private void TasksManager_Click(object sender, RoutedEventArgs e)
        {
            TasksViewer tv = new TasksViewer();
            tv.ShowDialog();

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
