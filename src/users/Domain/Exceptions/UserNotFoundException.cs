using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string email) : base($"User with email {email} was not found") { }

        public UserNotFoundException(int userId) : base($"User with id {userId} was not found") { }
    }
}
