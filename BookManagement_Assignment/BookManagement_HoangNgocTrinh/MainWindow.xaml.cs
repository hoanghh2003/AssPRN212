using Services;
using System.Windows;
using Repository.Entities;
using System.Windows.Controls;

namespace BookManagement_HoangNgocTrinh
{
    public partial class MainWindow : Window
    {
        private BookService _service = new();
        private int _userRole;

        public MainWindow(int userRole)
        {
            InitializeComponent();
            _userRole = userRole;
            LoadGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole != 1) // Check if the user is not an Administrator
            {
                MessageBox.Show("You do not have permission to update books.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Book selected = BookListDataGrid.SelectedItem as Book;
            if (selected == null)
            {
                MessageBox.Show("Please select a book before editing", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BookDetailWindow detail = new BookDetailWindow(selected.BookId, _userRole); // Pass the role
            detail.SelectedBook = selected;
            detail.ShowDialog();
            LoadGrid();
        }

        private void BookMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            BookListDataGrid.ItemsSource = null;
            BookListDataGrid.ItemsSource = _service.GetBooks(_userRole);
        }

        private void CreateBookButton_Click(object sender, RoutedEventArgs e)
        {
            BookDetailWindow detail = new BookDetailWindow(0, _userRole); // Pass the role
            detail.ShowDialog();
            if (detail.DialogResult == true)
            {
                LoadGrid();
            }
            else
            {
                MessageBox.Show("You have pressed Cancel");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void BookNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userRole != 1) // Check if the user is not an Administrator
            {
                MessageBox.Show("You do not have permission to delete books.", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Book selected = BookListDataGrid.SelectedItem as Book;

            if (selected == null)
            {
                MessageBox.Show("Please select a book before deleting", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Do you really want to DELETE?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            _service.DeleteBook(selected, _userRole); // Pass the role
            LoadGrid();
        }

        public void PerformSearch()
        {
            string name = BookNameTextBox.Text;
            string des = DescriptionTextBox.Text;
            var filter = _service.SearchBooks(name, des, _userRole); // Pass the role
            BookListDataGrid.ItemsSource = filter;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to quit?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}
