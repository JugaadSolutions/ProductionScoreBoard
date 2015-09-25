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
using PSB_OperatorApp.Views;
using Andonmanager;


namespace PSB_OperatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public String Location { get; set; }

      
        DashboardView Dashboard;
        
        public MainWindow()
        {
            InitializeComponent();
            BannerTextBlock.Text += Environment.NewLine + "Operator App";

            Dashboard = new DashboardView();
            DashboardGrid.Children.Clear();
            DashboardGrid.Children.Add(Dashboard);

            

          

          
            

           



        }
    }
}
