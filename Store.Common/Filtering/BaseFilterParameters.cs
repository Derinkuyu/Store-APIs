namespace Store.Common
{
    // Base class: generic search + sort fields shared by all filter DTOs
    public abstract class BaseFilterParameters
    {
        public string? Search { get; set; }       // full-text search across name/description
        public string? SortBy { get; set; }       // field name to sort by (e.g. "name", "price")
        public bool IsDescending { get; set; } = false;
    }
}
