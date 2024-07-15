using Repository;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService
    {
        // GUI gọi Service gọi Repository gọi DBCOntext gọi Table Thật Sự
        //Ta cần khai báo biến Repo
        private CategoryRepository _repo = new();

        public List<BookCategory> GetALlCatrgories()
        {
            return _repo.GetAll();
            //expression bodied
        }

        public BookCategory GetBookCategoryID(int id)
        {
            return _repo.GetCategoryId(id);
        }

        public void SaveBookCategory(BookCategory category)
        {
            if (category.BookCategoryId == 0)
            {
                category.BookCategoryId = _repo.GetAll().Max(c => c.BookCategoryId) + 1;
                _repo.Create(category);
            }
            else
            {
                _repo.Update(category);
            }
        }

        public void DeleteBookCategory(int id)
        {
            _repo.Delete(id);
        }

        public List<BookCategory> SearchCategroy(string BookGenreType, string Description)
        {
            return _repo.Search(BookGenreType, Description);
        }


        
    }
}
