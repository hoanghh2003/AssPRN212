using Repository;
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
            var user = _repository.GetUserByEmail(email);
            if (user != null && user.Password == password)
            {
                role = user.Role;
                return true;
            }
            role = -1;
            return false;
        }

        public bool CanCreate(int role)
        {
            return role == 1 || role == 2;
        }

        public bool CanEdit(int role)
        {
            return role == 1 || role == 2;
        }

        public bool CanDelete(int role)
        {
            return role == 1;
        }

        public bool CanView(int role)
        {
            return role == 1 || role == 2 || role == 3;
        }

        public bool CanSearch(int role)
        {
            return role == 1 || role == 2 || role == 3;
        }
    }
}