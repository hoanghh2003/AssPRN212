using Repository;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private readonly UserRepository _repository;

        public UserService()
        {
            _repository = new UserRepository();
        }

        public bool CheckUser(string email, string password, out int role)
        {
            var user = _repository.GetUserByEmail(email, password);
            if (user != null)
            {
                role = user.Role;
                return true;
            }
            role = -1;
            return false;
        }

        public bool CanCreate(int role)
        {
            return role == 1;
        }

        public bool CanEdit(int role)
        {
            return role == 1;
        }

        public bool CanDelete(int role)
        {
            return role == 1;
        }

        public bool CanView(int role)
        {
            return role == 1 || role == 2;
        }

        public bool CanSearch(int role)
        {
            return role == 1 || role == 2;
        }
         public List<UserAccount> getAll()
        {
            return _repository.GetAll();
        }

        public List<UserAccount> SearchBook(string name, string email)
        {
            return _repository.Search(name, email);
        }
        public void Delete (UserAccount account)
        {
            _repository.Delete(account);
        }
        public UserAccount GetTop1()
        {
            return _repository.GetTop1Menber();
        }
        

        public void  CreateUser (UserAccount account)
        {
            _repository.CreateUser(account);
        }
        public void UpdateUser (UserAccount account)
        {
            _repository.UpdateUser(account);
        }
        public bool CheckEmail (string email)
        {
           List<UserAccount> list = _repository.GetAll().ToList();
            foreach (UserAccount account in list)
            {
                if(account.Email.Equals(email)) return true;
            }
            return false;
        }
    }
}