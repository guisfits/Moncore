using FluentValidation.Results;
using Moncore.Domain.Validations;
using Moncore.Domain.ValueObjects;

namespace Moncore.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }

        public override ValidationResult Validate()
        {
            var validation = new UserValidator();
            return validation.Validate(this);
        }
    }
}
