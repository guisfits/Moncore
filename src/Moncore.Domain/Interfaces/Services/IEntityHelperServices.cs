namespace Moncore.Domain.Interfaces.Services
{
    public interface IEntityHelperServices
    {
        bool EntityHasProperties<T>(string fields);
    }
}
