using System;

namespace Moncore.Api.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }        
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}
