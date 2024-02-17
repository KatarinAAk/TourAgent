using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Xml.Linq;
using Microsoft.Office.Interop.Word;

namespace TourAgent
{
    /// <summary>
    /// Логика взаимодействия для GenerateReportWindow.xaml
    /// </summary>
    public partial class GenerateReportWindow : System.Windows.Window
    {
        private int employeeID; 
        public GenerateReportWindow(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate = startDatePicker.SelectedDate.Value;
            DateTime endDate = endDatePicker.SelectedDate.Value;

            Microsoft.Office.Interop.Word.DataTable reportData = GenerateReportFromDatabase(employeeID, startDate, endDate);

            if (reportData != null)
            {
                //dataGrid.ItemsSource = reportData.DefaultView;
            }
            else
            {
                MessageBox.Show("Не удалось сгенерировать отчет из базы данных.");
            }
        }

        private Microsoft.Office.Interop.Word.DataTable GenerateReportFromDatabase(int employeeID, DateTime startDate, DateTime endDate)
        {
            string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True"; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM CompletedTask WHERE EmployeeID = @EmployeeID AND CompletionDate >= @StartDate AND CompletionDate <= @EndDate;" +
                               "SELECT * FROM Sale WHERE EmployeeID = @EmployeeID AND SaleDate >= @StartDate AND SaleDate <= @EndDate";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                DataSet reportDataSet = new DataSet();

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(reportDataSet);
                    return (Microsoft.Office.Interop.Word.DataTable)reportDataSet.Tables[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Отчет об ошибке, генерируемый из базы данных: " + ex.Message);
                    return null;
                }
            }
        }

        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            SaveReportAsDoc((System.Data.DataTable)dataGrid.ItemsSource);
        }

        private void SaveReportAsDoc(System.Data.DataTable reportData)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Document doc = wordApp.Documents.Add();

            foreach (DataRow row in reportData.Rows)
            {
                string data = "";
                foreach (var item in row.ItemArray)
                {
                    data += item.ToString() + "\t";
                }
                doc.Content.InsertAfter(data + "\n");
            }

            object fileName = "Report.docx";
            doc.SaveAs2(ref fileName);
            doc.Close();
            wordApp.Quit();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            PofileWindow pofileWindow = new PofileWindow(employeeID);
            pofileWindow.Show();
            this.Close();
        }
    }
}
