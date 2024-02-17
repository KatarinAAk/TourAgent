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
    /// Логика взаимодействия для ManageTasksWindow.xaml
    /// </summary>
    public partial class ManageTasksWindow : Window
    {
        private int employeeID;

        public ManageTasksWindow(int employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
            LoadTasks();
        }

       private void LoadTasks()
        {
            string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True";

            string query = "SELECT TaskID, TaskDescription, TaskType, TaskDate FROM Task WHERE EmployeeID = @EmployeeID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);

                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        taskDataGrid.ItemsSource = dataTable.DefaultView;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка: " + ex.Message);
                    }
                }
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string taskDescription = taskDescriptionTextBox.Text;
            string taskType = taskTypeComboBox.SelectedItem.ToString();
            DateTime taskDate = taskDatePicker.SelectedDate ?? DateTime.Now;

            string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True";

            string query = "INSERT INTO Task (EmployeeID, TaskDescription, TaskType, TaskDate) VALUES (@EmployeeID, @TaskDescription, @TaskType, @TaskDate)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", employeeID);
                    command.Parameters.AddWithValue("@TaskDescription", taskDescription);
                    command.Parameters.AddWithValue("@TaskType", taskType);
                    command.Parameters.AddWithValue("@TaskDate", taskDate);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Задача успешно добавлена");
                            taskDataGrid.Items.Clear();
                            LoadTasks();
                        }
                        else
                        {
                            MessageBox.Show("Не удалось добавить задачу");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка: " + ex.Message);
                    }
                }
            }
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (taskDataGrid.SelectedItem != null && taskDataGrid.SelectedItem is DataRowView selectedTask)
            {
                int taskID = (int)selectedTask.Row["TaskID"];

                string connectionString = "Data Source=DESKTOP-OD7L183\\SQLEXPRESS;Initial Catalog=TourAgent;Integrated Security=True";

                string query = "UPDATE Task SET TaskStatus = 'Completed' WHERE TaskID = @TaskID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskID", taskID);

                        try
                        {
                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Задача успешно выполнена");
                                LoadTasks();
                            }
                            else
                            {
                                MessageBox.Show("Не удалось выполнить задачу");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Произошла ошибка: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для выполнения");
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