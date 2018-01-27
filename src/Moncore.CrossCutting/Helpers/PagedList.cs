using System;
using System.Collections.Generic;
using System.Linq;

namespace Moncore.CrossCutting.Helpers
{
    public class PagedList<T> : List<T>
    {
        public readonly int CurrentPage;
        public readonly int TotalPages;
        public readonly int PageSize;
        public readonly int TotalCount;

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        protected PagedList(List<T> items, int count, int page, int size)
        {
            TotalCount = count;
            PageSize = size;
            CurrentPage = page;
            TotalPages = (int) Math.Ceiling(count / (double) PageSize);
            AddRange(items);
        }

        public static PagedList<T> Create(IQueryable<T> source, int page, int size)
        {
            var count = source.Count();
            var items = source
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            return new PagedList<T>(items, count, page, size);
        }
    }
}
