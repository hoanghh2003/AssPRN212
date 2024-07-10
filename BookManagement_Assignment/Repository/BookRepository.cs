

using Microsoft.EntityFrameworkCore;

namespace Repository.Entities
{
    public class BookRepository
    {
        // clas này chính là cái Cabinet<Book> mình đã từng làm 
        // nó chauws các hàm CRUD Book
        // nó cần triệu gọi thằng DBContext, đại sứ Database
        // đang nắm chuỗi
        // Connnection String đang để ở appseting.json
        // các hàm CRUD() bên REpo này gọi BDContext Bookss để giúp thao tác trực tiếp trên table
        // Class Repo này lại bị gọi bởi clas Service// GUI -> Service -> Respository -> DbContext -> Database


        // khai bóa biến đại sứ DB
        private BookManagementDbContext _context;
        // ko new, lúc nào xài thì new ! new lại mỗi lần crud

        public List<Book> GetBooks()
        {
            _context = new BookManagementDbContext();
            //return _context.Books.ToList();
            //Lazy Loading : Không thèm lấy data của bookCategory 
            //Ko thèm chủ động join để cái thiện performance
            //Nếu muốn chủ động để láy data ngoài bảng khác thì hãy chấm thêm 1 hàm include
            return _context.Books.Include("BookCategory").ToList();
        }

        public void Create(Book x)
        {
            //Insert inot book Value(...)
            _context = new();
            _context.Books.Add(x); //_arr.Add(x)
            _context.SaveChanges();
        }

        public Book GetBookByID (int id)
        {
            _context = new();
            return _context.Books.FirstOrDefault(b => b.BookId == id);
        }

        public void AddBook (Book book)
        {
            _context.Books.Add(book);
        }

        public void Update(Book x)
        {
            //Update in book Value(...)
            _context = new();
            _context.Books.Update(x); //CUỐN SÁCH X PHẢI TỒN TẠI PK TRONG TABLE RỒI, CÓ SẴN RỒI
            _context.SaveChanges();
        }

        public void Delete(Book x)
        {
            _context = new();

            _context.Remove(x);
            _context.SaveChanges();

        }

        public List<Book> Search(string name, string description)
        {
            _context = new();
            return _context.Books.Where(b =>
            ( string.IsNullOrEmpty(name) || b.BookName.ToUpper().Contains(name.ToUpper())) && 
            (string.IsNullOrEmpty(description) || b.Description.ToUpper().Contains(description.ToUpper())))
                .ToList();
        }

    }
}
