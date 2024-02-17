using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private int employeeID;

        public EmployeeWindow(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
            InitializeEmployeeData();
        }
        private void InitializeEmployeeData()
        {
            string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True";

            string query = "SELECT FirstName, LastName, Position, Email FROM Employee WHERE EmployeeID = @EmployeeID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            firstNameTextBox.Text = reader["FirstName"].ToString();
                            lastNameTextBox.Text = reader["LastName"].ToString();
                            positionTextBox.Text = reader["Position"].ToString();
                            emailTextBox.Text = reader["Email"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Сотрудник не найден");
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка: " + ex.Message);
                        this.Close();
                    }
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = firstNameTextBox.Text;
            string lastName = lastNameTextBox.Text;
            string position = positionTextBox.Text;
            string email = emailTextBox.Text;
            string password = passwordBox.Password;

            string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Position = @Position, Email = @Email, Password = @Password WHERE EmployeeID = @EmployeeID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Position", position);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Информация о сотруднике успешно обновлена");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось обновить информацию о сотруднике");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка: " + ex.Message);
                    }
                }
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