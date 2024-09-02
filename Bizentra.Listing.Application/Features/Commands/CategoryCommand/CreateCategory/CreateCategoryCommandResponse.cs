using Bizentra.Listing.Application.Responses;

namespace Bizentra.Listing.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandResponse : BaseResponse
    {
        public CreateCategoryCommandResponse() : base()
        {

        }

        public CreateCategoryDto Category { get; set; } = default!;
    }
}
