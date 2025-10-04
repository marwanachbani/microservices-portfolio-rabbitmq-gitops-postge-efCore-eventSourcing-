namespace CatalogService.Events
{
    public class ProductCreatedEvent
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }
}
