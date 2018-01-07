using Moncore.Domain.Entities.Base;

namespace Moncore.Domain.Entities.UserAggregate
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }
    }
}
