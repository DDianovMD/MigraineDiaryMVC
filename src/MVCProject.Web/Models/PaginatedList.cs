using Microsoft.EntityFrameworkCore;

namespace MigraineDiary.Web.Models
{
    public class PaginatedList<T> : List<T> where T : class
    {
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            this.CurrentPageIndex = pageIndex;
            this.TotalPagesCount = (int)Math.Ceiling(count / (double)pageSize);
            this.HasPreviousPage = this.CurrentPageIndex > 1;
            this.HasNextPage = CurrentPageIndex < TotalPagesCount;

            this.AddRange(items);
        }

        public int CurrentPageIndex { get; private set; }

        public int TotalPagesCount { get; private set; }

        public bool HasPreviousPage { get; private set; }

        public bool HasNextPage { get; private set; }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
