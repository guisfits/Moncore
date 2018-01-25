using System.ComponentModel.DataAnnotations;

namespace Moncore.Api.Models
{
    public class PostForCreatedDto
    {
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
