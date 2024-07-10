using Repository.Entities;

namespace Services
{
    public class BookService
    {
        //Gui -> Service -> Repository                     -> DBContext -> DATAbase
        //                           CRUD trực tiếp table       Danh sách các table 3 cái DbSet
        //       Gọi CRUD để đẩy ên UI
        //       Nhận info từ UI đẩy xuống Repo

        //tên method thường dễ hiểu , gần với user
        // cần bookRepo để thao tác cơ sở dữ liệu
        private BookRepository  _book = new BookRepository();
        public List<Book> GetBooks()
        {
            return _book.GetBooks();
        }

        public  void AddBook(Book x)
        {
            _book.Create(x);
        }

        public void UpdateBook(Book x)
        {
            _book.Update(x);
        }
        public Book GetBookId(int id)
        {
            return _book.GetBookByID(id);
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
            {
                // Sách mới
                book.BookId = _book.GetBooks().Max(b => b.BookId) + 1;
                _book.AddBook(book);
            }
            else
            {
                // Cập nhật sách
                _book.Update(book);
            }
        }

        public void DeleteBook(Book x)
        {
            _book.Delete(x);
        }

        public List<Book> SearchBook(string name, string description)
        {
            return _book.Search(name, description);
        }
    }
}
