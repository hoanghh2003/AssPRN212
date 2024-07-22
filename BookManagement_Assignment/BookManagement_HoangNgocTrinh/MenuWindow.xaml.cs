using PE;
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

namespace BookManagement_HoangNgocTrinh
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private int _userRole;
        public MenuWindow(int role)
        {
            InitializeComponent();
            _userRole = role;
        }



        private void ManageBookButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow(_userRole);

            mainWindow.ShowDialog();
            this.Show();

        }

        private void ManageAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            WindowUser main = new(_userRole);
            main.ShowDialog();
            this.Show();

            

        }

        private void Load_Grid(object sender, RoutedEventArgs e)
        {
            if (_userRole == 2) {
                ManageAccountButton.IsEnabled = false;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            // Đóng MainWindow
            Application.Current.MainWindow = loginWindow;
            this.Close();
        }
    }
}
