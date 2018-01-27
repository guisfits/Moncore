namespace Moncore.CrossCutting.Helpers
{
    public class PaginationParameters
    {
        private const int MaxPageSize = 20;
        public int Page { get; set; } = 1;
        private int _pageSize = 10;
        public int Size
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
