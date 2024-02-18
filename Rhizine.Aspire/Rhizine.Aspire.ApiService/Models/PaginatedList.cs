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
        public bool IsEmpty => Items.Count == 0;

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

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public int IndexOf(T item)
        {
            return Items.IndexOf(item);
        }

        public void Sort(Comparison<T> comparison)
        {
            ((List<T>)Items).Sort(comparison);
        }

        public IEnumerable<T> Filter(Func<T, bool> predicate)
        {
            return Items.Where(predicate);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            Items.RemoveAt(index);
        }

        public void Add(T item)
        {
            Items.Add(item);
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > Items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            Items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return Items.Remove(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public T[] ToArray()
        {
            return Items.ToArray();
        }

        public List<T> ToList()
        {
            return Items.ToList();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}
