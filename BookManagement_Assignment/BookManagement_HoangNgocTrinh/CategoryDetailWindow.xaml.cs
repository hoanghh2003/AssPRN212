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

namespace BookManagement_HoangNgocTrinh
{
    /// <summary>
    /// Interaction logic for CategoryDetailWindow.xaml
    /// </summary>
    public partial class CategoryDetailWindow : Window
    {
        private CategoryService _categoryService = new();
        private BookCategory _selectedCategory;

        public CategoryDetailWindow()
        {
            InitializeComponent();
            this.Loaded += CategoryDetailWindow1_Loaded;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(BookGenreTypeTextBox.Text))
            {
                MessageBox.Show("Please enter a valid Genre Type", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var category = new BookCategory
            {
                BookGenreType = BookGenreTypeTextBox.Text,
                Description = DescriptionTextBox.Text,
            };

            try
            {
                _categoryService.SaveBookCategory(category);
                MessageBox.Show("Category saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
                ClearForm();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCategories()
        {
            var categories = _categoryService.GetALlCatrgories();
            BookCategoryListDataGrid.ItemsSource = categories;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedCategory == null)
            {
                MessageBox.Show("Please select a category to update.", "No Category Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate inputs
            if (string.IsNullOrWhiteSpace(BookGenreTypeTextBox.Text))
            {
                MessageBox.Show("Please enter a valid Genre Type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _selectedCategory.BookGenreType = BookGenreTypeTextBox.Text;
            _selectedCategory.Description = DescriptionTextBox.Text;

            try
            {
                _categoryService.SaveBookCategory(_selectedCategory);
                MessageBox.Show("Category updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCategory == null)
            {
                MessageBox.Show("Please select a category to delete.", "No Category Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _categoryService.DeleteBookCategory(_selectedCategory.BookCategoryId);
                MessageBox.Show("Category deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CategoryDetailWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }
        private void ClearForm()
        {
            BookGenreTypeTextBox.Clear();
            DescriptionTextBox.Clear();
            _selectedCategory = null;
        }

        private void BookCategoryListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookCategoryListDataGrid.SelectedItem is BookCategory selectedCategory)
            {
                _selectedCategory = selectedCategory;
                BookGenreTypeTextBox.Text = selectedCategory.BookGenreType;
                DescriptionTextBox.Text = selectedCategory.Description;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var bookGenreType = BookGenreTypeTextBox.Text;
            var description = DescriptionTextBox.Text;

            var results = _categoryService.SearchCategroy(bookGenreType, description);
            BookCategoryListDataGrid.ItemsSource = results;
        }
    }
}
