using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Options
{
    public class SmtpOptions
    {
        public const string SectionName = "Smtp";

        public string Host { get; set; } = "localhost";

        public int Port { get; set; } = 587;

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string SenderEmail { get; set; } = "noreply@example.com";

        public string SenderName { get; set; } = "InnoShop";

        public bool UseSsl { get; set; } = false;
    }
}
