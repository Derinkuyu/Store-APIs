namespace Store.Common
{
    // Inherits: Search, SortBy, IsDescending from BaseFilterParameters
    // Inherits: PageNumber, PageSize (capped at 50) from PaginationParameters
    public class ProductFilterParameters : BaseFilterParameters
    {
        // Product-specific filters
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Pagination (capped at MaxPageSize = 50)
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
