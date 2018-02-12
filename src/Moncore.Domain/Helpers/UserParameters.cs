using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;
using System.Collections.Generic;

namespace Moncore.Domain.Helpers
{
    public class UserParameters : PaginationParameters<User>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}