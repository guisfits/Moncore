using Moncore.Domain.Entities;

namespace Moncore.Domain.Helpers
{
    public abstract class PaginationParameters<TEntity> where TEntity : Entity
    {
        private const int MaxPageSize = 20;
        public int Page { get; set; } = 1;
        private int _pageSize = 10;
        public int Size
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string Search { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string Fields { get; set; }
    }
}