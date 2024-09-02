using Bizentra.Listing.Application.Features.Categories.Commands.CreateCategory;
using MediatR;

namespace Bizentra.Listing.Application.Features.Commands.CategoryCommand.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CreateCategoryCommandResponse>
    {
        public string Name { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
