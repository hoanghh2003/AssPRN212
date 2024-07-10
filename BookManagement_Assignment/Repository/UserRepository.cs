using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository
    {
        private List<UserAccount> users;
        private BookManagementDbContext _context;

        public UserAccount GetUserByEmail(string email)
        {
            _context = new BookManagementDbContext();
            return _context.UserAccounts.FirstOrDefault(b => b.Email == email);
        }
    }
}
