using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailConfirmationTokenService : IEmailConfirmationTokenService
    {
        public string GenerateToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(tokenBytes);
        }

        public bool ValidateToken(string token, string storedToken, DateTime? expires)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(storedToken))
                return false;

            if (expires == null || expires < DateTime.UtcNow)
                return false;

            return token == storedToken;
        }
    }
}
