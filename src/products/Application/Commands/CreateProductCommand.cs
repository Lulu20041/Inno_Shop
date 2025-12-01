using System;
using MediatR;

namespace Application.Commands
{
    public record CreateProductCommand : IRequest<int>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public double Price { get; init; }
        public int UserId { get; init; } 
    }
}
