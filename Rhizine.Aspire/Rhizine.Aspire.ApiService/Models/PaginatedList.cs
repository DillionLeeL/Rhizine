using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading;

namespace Rhizine.Aspire.ApiService.Models
{
    public class PaginatedList<T>
    {
        public IList<T> Items { get; private set; }
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public bool IsEmpty => !Items.Any();

        private PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items.ToList();
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentException("PageIndex must be greater than 0.", nameof(pageIndex));
            }

            if (pageSize < 1)
            {
                throw new ArgumentException("PageSize must be greater than 0.", nameof(pageSize));
            }

            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public T GetItemAt(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            return Items[index];
        }
    }
}
