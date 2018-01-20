using System.Collections.Generic;
using Moncore.Domain.Entities.Base;

namespace Moncore.Domain.Entities
{
    public class Album : BaseEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; }
    }
}
