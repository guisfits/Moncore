using System;

namespace Moncore.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}