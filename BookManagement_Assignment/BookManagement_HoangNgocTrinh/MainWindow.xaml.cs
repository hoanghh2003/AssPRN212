using Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using Repository.Entities;
namespace BookManagement_HoangNgocTrinh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Gui gọi service - Repo - DBContext -> DataBase
        private BookService _service = new();
       
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Book selected = BookListDataGrid.SelectedItem as Book;
            //ráng convert thử 1 dòng trong grid thành book
            // nhưng có thẻ kh tành công , thì gán null, toán tửu as kĩ thuật ép
            // kiểu an toàn kh exception

            // Book selected = (Book) BookListDataGrid.SelectedItem;
            //if (selected != null)
            //{
            //    MessageBox.Show(selected.BookId + " | " + selected.BookName);
            //}
            if (selected == null)
            {
                MessageBox.Show("Please select a book before editing", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BookDetailWindow detail = new();
            detail.SelectedBook = selected;
            detail.ShowDialog(); //bên details load sách vào các ô
            LoadGrid();


        }

        private void BookMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Hàm này sẽ được gọi mỗi khi hàm đc new , được render
            //Đổ vào Guid để đỡ phải nhấn nút
            //Đổ vào grid sẽ xuất hiển nhiều lần
            //Mở màn hinhd , thêm mới 1 cuốn sahcs đổ lại lưới
            LoadGrid();
        }

        private void LoadGrid()
        {
            BookListDataGrid.ItemsSource = null; //Xóa trắng list, Không lấy nguồn data nào
            
            BookListDataGrid.ItemsSource = _service.GetBooks();

        }

        private void CreateBookButton_Click(object sender, RoutedEventArgs e)
        {
            BookDetailWindow detail = new();
            detail.ShowDialog();
            if (detail.DialogResult == true)
            {
                LoadGrid();
            }
            //F5 lại DataGrid
            else
                MessageBox.Show("Bạn đã nhấn Cancel");
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
            PerformSearch() ;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Book selected = BookListDataGrid.SelectedItem as Book;

            if (selected == null) 
            {
                MessageBox.Show("Please select book before deleting", "Select one", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Do you really want to DELETE?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            _service.DeleteBook(selected);
            LoadGrid();
        }

        public void PerformSearch()
        {
            string name = BookNameTextBox.Text;
            string des = DescriptionTextBox.Text;
            var filter = _service.SearchBook(name, des);
            BookListDataGrid.ItemsSource = filter;
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to quit?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            Application.Current.Shutdown();
        }
    }
}