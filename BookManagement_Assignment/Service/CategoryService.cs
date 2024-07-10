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

        
    }
}
