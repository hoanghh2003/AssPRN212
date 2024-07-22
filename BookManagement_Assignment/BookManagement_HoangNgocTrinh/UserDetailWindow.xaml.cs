using Microsoft.VisualBasic.ApplicationServices;
using Repository.Entities;

using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PE
{
    /// <summary>
    /// Interaction logic for UserDetailWindow.xaml
    /// </summary>
    public partial class UserDetailWindow : Window
    {
        UserService userService = new();
        public UserDetailWindow()
        {
            InitializeComponent();

        }
        public UserAccount? SelecteAccount { private get; set; }



        private void UserDetailWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var roleMapping = new Dictionary<int, string>
    {
        { 3, "Customer" },
        { 2, "Staff" },
        { 1, "Admin" }
    };
            
            var priceList = new List<string> { "Customer", "Staff", "Admin" };
            UserRoleComboBox.ItemsSource = priceList;
            WindowModeLabel.Content = "Add New  User";
            if (SelecteAccount != null)
            {
                // ta kh cho user edit book id ở mode edit
                // ta cập nhật lại label dể thông báo rõ màn hình gì với user
                WindowModeLabel.Content = "Update A User";
                FullNameTextBox.Text = SelecteAccount.FullName.ToString();
                EmailTextBox.Text = SelecteAccount.Email.ToString();
                PasswordTextBox.Text = SelecteAccount.Password.ToString();


                UserRoleComboBox.SelectedValue = SelecteAccount.Role.ToString();
                // quan trọng , nếu kh mọi cuốn sách edit đều về
                // category  đầu tiên 1-fiction type
                UserRoleComboBox.SelectedValue = roleMapping.ContainsKey(SelecteAccount.Role) ? roleMapping[SelecteAccount.Role] : null;
            }
           

        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate non-empty fields
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                string.IsNullOrWhiteSpace(UserRoleComboBox.Text))
            {
                System.Windows.MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!EmailTextBox.Text.EndsWith("@bookstore.com"))
            {
                System.Windows.MessageBox.Show("Email must be in the format of '@bookstore.com'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (FullNameTextBox.Text.Length < 5 || !char.IsUpper(FullNameTextBox.Text[0]))
            {
                System.Windows.MessageBox.Show("Full name must start with an uppercase letter and be at least 5 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate email format


            UserAccount user = new UserAccount();
            user.MemberId = userService.GetTop1().MemberId + 1;
            user.FullName = FullNameTextBox.Text;
            user.Email = EmailTextBox.Text;
            user.Password = PasswordTextBox.Text;

            var roleMapping = new Dictionary<string, int>
    {
        { "Customer", 3 },
        { "Staff", 2 },
        { "Admin", 1 }
    };

            if (roleMapping.TryGetValue(UserRoleComboBox.Text, out int roleValue))
            {
                user.Role = roleValue;
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid role selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (SelecteAccount != null)
            {
                user.MemberId = SelecteAccount.MemberId;
                userService.UpdateUser(user);
            }
            else
            {
                if (userService.CheckEmail(user.Email))
                {
                    System.Windows.MessageBox.Show("Email is already in the system.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                userService.CreateUser(user);
            }
            WindowUser main = new WindowUser(SelecteAccount.Role);
            main.Show();
            this.Close();
            System.Windows.MessageBox.Show("User saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
