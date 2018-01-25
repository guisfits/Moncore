using System;

namespace Moncore.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
