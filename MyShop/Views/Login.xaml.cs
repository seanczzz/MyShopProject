using Microsoft.Data.SqlClient;
using MyShop.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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




namespace MyShop.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private string _username;
        private string _password;

        public Login()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string password = AppConfig.GetPassword();
            string username = AppConfig.GetValue(AppConfig.Username);
            try
            {
                PasswordBox.Password = password;
                UserNameBox.Text = username;
            }
            catch {
                
                AppConfig.SetPassword(password);
                AppConfig.SetValue(AppConfig.Username, username);
            }

        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UserNameBox.Text;
                string password = PasswordBox.Password;

                string connectionString = AppConfig.ConnectionString(username, password);

                //var connection = new SqlConnection(connectionString);

                var connection = await Task.Run(() =>
                {
                    var _connection = new SqlConnection(connectionString);

                    try
                    {
                        _connection.Open();
                    }
                    catch (Exception ex)
                    {

                        _connection = null;
                    }

                    // Test khi chạy quá nhanh
                    //System.Threading.Thread.Sleep(3000);
                    return _connection;
                });

                if (connection != null)
                {
                    AppConfig.SetValue("Username", username);
                    AppConfig.SetPassword(password);
                    

                    DB.Instance.ConnectionString = connectionString;


                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Wrong username or password", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when login");
            }
        }
    }
}
