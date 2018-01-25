using System.ComponentModel.DataAnnotations;

namespace Moncore.Api.Models
{
    public class PostForCreatedByUserDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
