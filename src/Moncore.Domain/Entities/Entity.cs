using System;
using FluentValidation.Results;

namespace Moncore.Domain.Entities
{
    public abstract class Entity
    {
        public string Id { get; set; }
        public abstract ValidationResult Validate();
    }
}