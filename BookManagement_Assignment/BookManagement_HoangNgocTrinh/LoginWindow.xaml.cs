using Services;
using System.Windows;
using System.Windows.Controls;

namespace BookManagement_HoangNgocTrinh
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserService _userService;

        public LoginWindow()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;

            if (_userService.CheckUser(email, password, out int role))
            {
                MessageBox.Show("Login Successful !!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                MainWindow mainWindow = new MainWindow(role); // Pass the role to MainWindow
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Email or Password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
