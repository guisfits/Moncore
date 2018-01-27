using FluentValidation.Results;
using Moncore.Domain.Validations;

namespace Moncore.Domain.Entities
{
    public class Post : Entity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public override ValidationResult Validate()
        {
            var validation = new PostValidator();
            return validation.Validate(this);
        }
    }
}
