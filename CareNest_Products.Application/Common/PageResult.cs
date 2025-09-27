namespace CareNest_Products.Application.Common
{
    /// <summary>
    /// Paged result wrapper
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class PageResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PageResult(IEnumerable<T> data, int totalCount, int pageSize, int pageIndex)
        {
            Data = data;
            TotalCount = totalCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}
