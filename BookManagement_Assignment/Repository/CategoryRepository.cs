using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Repository
{
    public class CategoryRepository
    {
        //Thằng nay chính là cái cabinet nhma chỉ chơi vs 1 table là bookCategory theo nguyên lí S của SOLID
        //Dĩ nhiên nó lại nhờ vả đại sứ database DbContext 
        //Dĩ nhiên class này là chứa các mà CRUD table category
        //Tên của các hàm ở đây rất gần DB cho nên thường gắn với 4 Câu SQL cơ bản: Select, Update, Delete, Insert
        private BookManagementDbContext _context;
        public List<BookCategory> GetAll()
        {
            _context = new BookManagementDbContext();
            return _context.BookCategories.ToList();
        }
        //Lấy đc hết Category , gồm 3 cột , CateId, GenreType, Desc
         public BookCategory GetCategoryId(int id)
         {
            _context = new BookManagementDbContext();
            return _context.BookCategories.FirstOrDefault(b => b.BookCategoryId == id);
         }

        public void Create (BookCategory x)
        {
            _context = new BookManagementDbContext();
            _context.BookCategories.Add(x);
            _context.SaveChanges();
        }

        public void Update (BookCategory category)
        {
            var existingCategory = _context.BookCategories.FirstOrDefault(c => c.BookCategoryId == category.BookCategoryId);
            if (existingCategory != null)
            {
                existingCategory.BookGenreType = category.BookGenreType;
                existingCategory.Description = category.Description;
                _context.SaveChanges();
            }
        }
        
        public void Delete (int id)
        {
            _context = new BookManagementDbContext();
            var category = GetCategoryId(id);
            if (category != null)
            {
                _context.BookCategories.Remove(category);
                _context.SaveChanges();
            }
        }
        public List<BookCategory> Search(string bookGenreType, string description)
        {
            _context = new BookManagementDbContext();
            return _context.BookCategories.Where(c =>
                (string.IsNullOrEmpty(bookGenreType) || c.BookGenreType.ToUpper().Contains(bookGenreType.ToUpper())) &&
                (string.IsNullOrEmpty(description) || c.Description.ToUpper().Contains(description.ToUpper())))
                .ToList();
        }
    }
}
