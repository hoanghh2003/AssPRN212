using Microsoft.Identity.Client.NativeInterop;
using Microsoft.VisualBasic.ApplicationServices;
using Services;
using System.Windows;

namespace BookManagement_HoangNgocTrinh
{
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
            string password = PasswordTextBox.Password;

            if (_userService.CheckUser(email, password, out int role))
            {
                if (role != 3)
                {
                    MessageBox.Show("Login Successful !!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    MainWindow mainWindow = new MainWindow(role); // Pass the role to MainWindow
                    mainWindow.Show();
                    this.Close();
                }else MessageBox.Show("You have no permission to acccess this function", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

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
    }
}
