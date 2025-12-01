using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailConfirmationTokenService
    {
        string GenerateToken();

        bool ValidateToken(string token, string storedToken, DateTime? expires);
    }
}
