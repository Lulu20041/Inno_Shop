using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    failure => failure.Key,
                    failure => failure.Select(x => x.ErrorMessage).ToArray()
                );
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
