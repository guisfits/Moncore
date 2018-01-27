using FluentValidation;
using Moncore.Domain.Entities;

namespace Moncore.Domain.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(c => c.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(c => c.Name)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(c => c.Username)
                .NotEmpty();
        }
    }
}
