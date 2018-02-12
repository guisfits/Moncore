using Moncore.Domain.Entities;

namespace Moncore.Domain.Helpers
{
    public class PostParameters : PaginationParameters<Post>
    {
        public string Title { get; set; }   
        public string Body { get; set; }
    }
}