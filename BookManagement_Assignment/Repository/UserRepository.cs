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

        public UserAccount GetUserByEmail(string email, string password)
        {
            _context = new BookManagementDbContext();
            return _context.UserAccounts.FirstOrDefault(b => b.Email == email && b.Password == password);
        }

        public UserAccount GetOne(string email, string password)
        {

            _context = new BookManagementDbContext();
            return _context.UserAccounts.FirstOrDefault(e => e.Email == email && e.Password == password);
            // so sánh 2 cuỗi dùng == ,  bản chất là so giá trị thực sự từng  kí tự có == nhua hay ko 
            // vì chuỗi là kiểu tham chiếu
        }

        public List<UserAccount> GetAll()
        {
            _context = new BookManagementDbContext();
            return _context.UserAccounts.ToList();
        }
        public List<UserAccount> Search(string name, string email)
        {
            // kh quan tâm hoa thường ta chuyển về thường rồi so sánh
            _context = new();
            return _context.UserAccounts.Where(b =>
            (string.IsNullOrEmpty(name) || b.FullName.ToLower().Contains(name.ToLower())) &&
            (string.IsNullOrEmpty(email) || b.Email.ToLower().Contains(email.ToLower()))).ToList();
        }
        public void Delete(UserAccount user)
        {
            _context = new();

            _context.Remove(user);
            _context.SaveChanges();

        }
        public UserAccount GetTop1Menber()
        {
            _context = new();
            return _context.UserAccounts
                       .OrderByDescending(u => u.MemberId)
                       .FirstOrDefault();
        }
        public void CreateUser(UserAccount user)
        {
            _context = new();
            _context.UserAccounts.Add(user);
            _context.SaveChanges(true);
        }
        public void UpdateUser(UserAccount user)
        {

            _context = new();
            _context.UserAccounts.Update(user);
            _context.SaveChanges(true);
        }
    }
}
