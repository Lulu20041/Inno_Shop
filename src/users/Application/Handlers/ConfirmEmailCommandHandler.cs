using Application.Commands;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailConfirmationTokenService tokenService;

        public ConfirmEmailCommandHandler(
            IUserRepository userRepository,
            IEmailConfirmationTokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }
        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.Email))
                throw new ValidationException("Token and email are required.");

            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
                throw new UserNotFoundException($"User with email {request.Email} not found.");

            if (!tokenService.ValidateToken(request.Token, user.EmailConfirmationToken, user.EmailTokenExpires))
                throw new ValidationException("Invalid or expired token.");

            user.IsEmailConfirmed = true;
            user.IsActive = true;
            user.EmailConfirmationToken = string.Empty;
            user.EmailTokenExpires = null;

            await userRepository.UpdateAsync(user, cancellationToken);

            return Unit.Value;
        }
    }
}
