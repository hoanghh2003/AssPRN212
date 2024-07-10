using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
