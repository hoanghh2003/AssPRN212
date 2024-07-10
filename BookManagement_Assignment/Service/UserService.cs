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
        private readonly UserRepository  _repository;
        public UserService()
        {
            _repository = new UserRepository();
        }

        public bool CheckUser(string email, string password)
        {
            var user = _repository.GetUserByEmail(email);
            if( user != null && user.Password == password) return true;
            return false;
        }
    }
}
