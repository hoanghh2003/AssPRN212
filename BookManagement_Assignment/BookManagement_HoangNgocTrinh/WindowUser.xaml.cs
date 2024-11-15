﻿using BookManagement_HoangNgocTrinh;
using Repository.Entities;

using Services;
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

namespace PE
{
    /// <summary>
    /// Interaction logic for WindowUser.xaml
    /// </summary>
    public partial class WindowUser : Window
    {

        UserService userService = new();
        private int role;
        public WindowUser(int role)
        {
            InitializeComponent();
            this.role = role;
        }

        private void UserAccountDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UserMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(role != 1) {
                CreateButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                UpdateButton.IsEnabled = false;
            }
            Load_Grid();
        }
        private void Load_Grid()
        {
            UserAccountDataGrid.ItemsSource = null;
            UserAccountDataGrid.ItemsSource = userService.getAll();
        }

        private void perform_Change()
        {
            string name = UserNameTextBox.Text;
            string email = EmailTextBox.Text;
            List<UserAccount> list = userService.SearchBook(name, email);
            if (list.Count == 0)
            {
                MessageBox.Show("Not found any account", "Find", MessageBoxButton.OK, MessageBoxImage.Warning);
                UserAccountDataGrid.ItemsSource = userService.getAll();
            }
            else { UserAccountDataGrid.ItemsSource = list; }

        }

        private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            perform_Change();
        }

        private void UserEmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            perform_Change();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserAccountDataGrid.SelectedItem is UserAccount selected)
            {
                if (selected.Role == 1)
                {
                    MessageBox.Show("Not allows to delete account with admin role", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var result = MessageBox.Show($"Do you want to delete '{selected.FullName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No) return;
                if (result == MessageBoxResult.Yes)
                {
                    userService.Delete(selected);
                    Load_Grid();

                }
                else
                {
                    MessageBox.Show("Please select a book to delete.", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            UserDetailWindow detail = new UserDetailWindow();
            // render()

            this.Hide();
            detail.ShowDialog();
            if (detail.DialogResult == true)
            {
                Load_Grid();
            }
            else
            {
                MessageBox.Show("You have pressed Cancel");
            }
            this.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserAccountDataGrid.SelectedItem is UserAccount selected)
            {
                if (selected.Role == 1)
                {
                    MessageBox.Show("Not allows to update account with admin role", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                UserDetailWindow main = new UserDetailWindow();
                main.SelecteAccount = selected;
                // 2 chàng 1 nàng
                // trước khi render, chuyển cuốn sách đã chọn bên kia
                main.ShowDialog(); // ben main load sách vòa các ô 
                Load_Grid();
            }
            else
            {
                // ko chọn sách mà đòi edit 
                MessageBox.Show("Please select a user to update.", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MenuWindow main1 = new(role);
            main1.Show();
        }

        private void SearchButtonBook_Click(object sender, RoutedEventArgs e)
        {
            perform_Change();
        }
    }

}
