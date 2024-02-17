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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TourAgent
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            try
            {
                string query = "SELECT EmployeeID FROM Employee WHERE Email = @Email AND Password = @Password";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                        command.Parameters.Add("@Password", SqlDbType.VarChar).Value = password;
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            int employeeID = Convert.ToInt32(result);
                            PofileWindow profileWindow = new PofileWindow(employeeID);
                            profileWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Неправильный email или пароль. Попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка проверки учетных данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}


