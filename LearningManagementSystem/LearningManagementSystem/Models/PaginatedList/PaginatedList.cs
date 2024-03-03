namespace LearningManagementSystem.Models.PaginatedList
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }

        public PaginatedList(List<T> item, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPage = (int)Math.Ceiling((double)count / pageSize);
            AddRange(item);
        }
        public static PaginatedList<T> Create(IQueryable<T> sources, int pageIndex, int pageSize)
        {
            var count = sources.Count();
            var items = sources.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
