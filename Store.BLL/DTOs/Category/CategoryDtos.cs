namespace Store.BLL
{
    public class CategoryCreateDto
    {
        /*------------------------------------------------------------------*/
        public required string Name { get; set; }
        public string? Description { get; set; }
    }

    public class CategoryEditDto
    {
        /*------------------------------------------------------------------*/
        public required string Name { get; set; }
        public string? Description { get; set; }
    }

    public class CategoryReadDto
    {
        /*------------------------------------------------------------------*/
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
