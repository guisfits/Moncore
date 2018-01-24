using System.ComponentModel.DataAnnotations;

namespace Moncore.Api.Models
{
    public class UserForCreatedDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Website { get; set; }
    }
}
