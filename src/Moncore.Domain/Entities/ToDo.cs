using Moncore.Domain.Entities.Base;

namespace Moncore.Domain.Entities
{
    public class ToDo : BaseEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}
