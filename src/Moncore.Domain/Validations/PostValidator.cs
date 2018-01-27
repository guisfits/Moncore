using FluentValidation;
using Moncore.Domain.Entities;

namespace Moncore.Domain.Validations
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotEqual(c => c.Id);

            RuleFor(c => c.Body)
                .NotEmpty()
                .NotEqual(c => c.Title);

            RuleFor(c => c.Title)
                .NotEmpty();
        }
    }
}
