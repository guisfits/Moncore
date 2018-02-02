using Moncore.CrossCutting.Helpers;

namespace Moncore.Api.Helpers
{
    public class PaginationParametersFiltersForUser : PaginationParameters
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
