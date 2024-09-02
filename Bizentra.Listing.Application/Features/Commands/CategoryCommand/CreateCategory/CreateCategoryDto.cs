namespace Bizentra.Listing.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryDto
    {
        public Guid CategoryId { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
