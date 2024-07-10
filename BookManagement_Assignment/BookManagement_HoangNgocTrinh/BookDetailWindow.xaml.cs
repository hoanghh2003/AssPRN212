using Repository.Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for BookDetailWindow.xaml
    /// </summary>
    public partial class BookDetailWindow : Window
    {

        private CategoryService _cateservice = new();
        private BookService _bookService = new();
        private int _bookId = new();

        public Book SelectedBook { get; set; } = null;
        public BookDetailWindow(int bookId = 0)
        {
            InitializeComponent();
            _bookService = new BookService();
            _bookId = bookId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Đổ data vào combo Box bất chấp tạo mới hayedit, Xài có 1 lần việc đổ, ko cần tách hàm
            //Dĩ nhiê cần gọi CategoryService trợ giúp
            BookCategoryIdComboBox.ItemsSource = _cateservice.GetALlCatrgories();
            BookCategoryIdComboBox.DisplayMemberPath = "BookGenreType";
            BookCategoryIdComboBox.SelectedValuePath = "BookCategoryId";


            BookCategoryIdComboBox.SelectedValue = 1;
            WindowModeLabel.Content = "Create A New Book";
            if (SelectedBook != null)
            {
                WindowModeLabel.Content = "Update A Book";
                BookIdTextBox.Text = SelectedBook.BookId.ToString();
                BookNameTextBox.Text = SelectedBook.BookName.ToString();
                DescriptionTextBox.Text = SelectedBook.Description.ToString();
                PublicationDateDatePicker.Text = SelectedBook.PublicationDate.ToString();
                QuantityTextBox.Text = SelectedBook.Quantity.ToString();
                PriceTextBox.Text = SelectedBook.Price.ToString();
                AuthorTextBox.Text = SelectedBook.Author.ToString();
                BookCategoryIdComboBox.SelectedValue = SelectedBook.BookCategoryId.ToString();
                BookIdTextBox.IsEnabled = false;
            }
        }

        private void PopulateBookDetails(Book book)
        {
            BookIdTextBox.Text = book.BookId.ToString();
            BookNameTextBox.Text = book.BookName;
            DescriptionTextBox.Text = book.Description;
            PublicationDateDatePicker.SelectedDate = book.PublicationDate;
            QuantityTextBox.Text = book.Quantity.ToString();
            PriceTextBox.Text = book.Price.ToString();
            AuthorTextBox.Text = book.Author;
            BookCategoryIdComboBox.SelectedValue = book.BookCategoryId;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate BookId
            if (!int.TryParse(BookIdTextBox.Text, out int bookId))
            {
                MessageBox.Show("Please enter a valid Book ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate BookName
            if (string.IsNullOrWhiteSpace(BookNameTextBox.Text))
            {
                MessageBox.Show("Book Name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate Quantity
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0 || quantity >= 4_000_000)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate Price
            if (!double.TryParse(PriceTextBox.Text, out double price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate BookCategoryId
            if (!int.TryParse(BookCategoryIdComboBox.SelectedValue.ToString(), out int bookCategoryId))
            {
                MessageBox.Show("Please select a valid Book Category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create and populate Book object
            Book x = new Book
            {
                BookId = bookId,
                BookName = BookNameTextBox.Text,
                Description = DescriptionTextBox.Text,
                PublicationDate = PublicationDateDatePicker.SelectedDate ?? DateTime.Now,
                Quantity = quantity,
                Price = price,
                Author = AuthorTextBox.Text,
                BookCategoryId = bookCategoryId
            };

            // Add or update the book
            if (SelectedBook != null)
            {
                _bookService.UpdateBook(x);
            }
            else
            {
                _bookService.AddBook(x);
            }

            DialogResult = true;
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();

        }
    }
}
