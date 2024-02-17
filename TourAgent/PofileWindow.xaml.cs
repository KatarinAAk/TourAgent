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
using System.Windows.Shapes;

namespace TourAgent
{
    /// <summary>
    /// Логика взаимодействия для PofileWindow.xaml
    /// </summary>
    public partial class PofileWindow : Window
    {
        private int employeeID;
        public PofileWindow(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWindow employeeWindow = new EmployeeWindow(employeeID);
            employeeWindow.Show();
            this.Close();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            ManageTasksWindow manageTasksWindow = new ManageTasksWindow(employeeID);
            manageTasksWindow.Show();
            this.Close();
        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            AddSaleWindow addSaleWindow = new AddSaleWindow(employeeID);
            addSaleWindow.Show();
            this.Close();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            GenerateReportWindow generateReportWindow = new GenerateReportWindow(employeeID);
            generateReportWindow.Show();
            this.Close();
        }
    }
}
