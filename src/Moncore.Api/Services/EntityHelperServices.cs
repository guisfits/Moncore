using System.Reflection;
using Moncore.CrossCutting.Extensions;
using Moncore.Domain.Interfaces.Services;

namespace Moncore.Api.Services
{
    public class EntityHelperService : IEntityHelperServices
    {
        public bool EntityHasProperties<T>(string fields)
        {
            if (fields.IsNullEmptyOrWhiteSpace())
                return true;

            var filedsAfterSplit = fields.Split(',');
            foreach (var field in filedsAfterSplit)
            {
                var propertyName = field.Trim();
                var property = typeof(T).GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                if (property == null)
                    return false;
            }

            return true;
        }
    }
}
