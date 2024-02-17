using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для AddSaleWindow.xaml
    /// </summary>
    public partial class AddSaleWindow : Window
    {
        private int employeeID; 

        public AddSaleWindow(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
            DisplayEmployeeTours(); 
        }

        private void AddSaleButton_Click(object sender, RoutedEventArgs e)
        {
            string clientName = clientNameTextBox.Text;
            int tourPackageID = int.Parse(tourPackageIDTextBox.Text);
            DateTime saleDate = saleDatePicker.SelectedDate.Value;

            if (AddSaleToDatabase(clientName, tourPackageID, saleDate))
            {
                MessageBox.Show("Распродажа успешно завершена!");
                clientNameTextBox.Text = "";
                tourPackageIDTextBox.Text = "";
                saleDatePicker.SelectedDate = DateTime.Today;
            }
            else
            {
                MessageBox.Show("Не удалось добавить продажу в базу данных.");
            }
        }

        private bool AddSaleToDatabase(string clientName, int tourPackageID, DateTime saleDate)
        {
            string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Sale (EmployeeID, ClientName, TourPackageID, SaleDate) " +
                               "VALUES (@EmployeeID, @ClientName, @TourPackageID, @SaleDate)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                command.Parameters.AddWithValue("@ClientName", clientName);
                command.Parameters.AddWithValue("@TourPackageID", tourPackageID);
                command.Parameters.AddWithValue("@SaleDate", saleDate);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при добавлении продажи в базу данных: " + ex.Message);
                    return false;
                }
            }
        }

        private DataTable GetEmployeeToursFromDatabase(int employeeID)
        {
            string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True"; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Sale WHERE EmployeeID = @EmployeeID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);

                DataTable toursTable = new DataTable();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    toursTable.Load(reader);
                    return toursTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при извлечении туров сотрудника из базы данных: " + ex.Message);
                    return null;
                }
            }
        }

        private void DisplayEmployeeTours()
        {
            DataTable employeeTours = GetEmployeeToursFromDatabase(employeeID);

            if (employeeTours != null)
            {
                tourDataGrid.ItemsSource = employeeTours.DefaultView;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            PofileWindow pofileWindow = new PofileWindow(employeeID);
            pofileWindow.Show();
            this.Close();
        }
    }
}