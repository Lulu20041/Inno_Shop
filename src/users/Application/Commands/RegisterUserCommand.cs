using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public record RegisterUserCommand : IRequest
    {
        public string Name { get; init; }

        public string Email { get; init; }

        public string Password { get; init; }
    }
}
