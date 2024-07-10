using Repository.Entities;
using Services;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BookManagement_HoangNgocTrinh
{
    public partial class BookDetailWindow : Window
    {
        private CategoryService _cateservice = new();
        private BookService _bookService = new();
        private int _bookId;
        private int _userRole;

        public Book SelectedBook { get; set; } = null;

        public BookDetailWindow(int bookId = 0, int userRole = 1)
        {
            InitializeComponent();
            _bookService = new BookService();
            _bookId = bookId;
            _userRole = userRole;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookCategoryIdComboBox.ItemsSource = _cateservice.GetALlCatrgories();
            BookCategoryIdComboBox.DisplayMemberPath = "BookGenreType";
            BookCategoryIdComboBox.SelectedValuePath = "BookCategoryId";

            BookCategoryIdComboBox.SelectedValue = 1;
            WindowModeLabel.Content = "Create A New Book";
            if (SelectedBook != null)
            {
                WindowModeLabel.Content = "Update A Book";
                PopulateBookDetails(SelectedBook);
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
            if (string.IsNullOrWhiteSpace(BookNameTextBox.Text) || BookNameTextBox.Text.Length < 5 || BookNameTextBox.Text.Length > 90 || !IsTitleCase(BookNameTextBox.Text))
            {
                MessageBox.Show("Book Name must be between 5 and 90 characters, and each word must begin with a capital letter.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate Quantity
            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0 || quantity >= 4_000_000)
            {
                MessageBox.Show("Please enter a valid quantity (0 <= quantity < 4,000,000).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate Price
            if (!double.TryParse(PriceTextBox.Text, out double price) || price < 0 || price >= 4_000_000)
            {
                MessageBox.Show("Please enter a valid price (0 <= price < 4,000,000).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            try
            {
                if (SelectedBook != null)
                {
                    _bookService.UpdateBook(x, _userRole);
                }
                else
                {
                    _bookService.AddBook(x, _userRole);
                }

                DialogResult = true;
                this.Close();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private bool IsTitleCase(string str)
        {
            var words = str.Split(' ');
            foreach (var word in words)
            {
                if (char.IsLower(word[0]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
