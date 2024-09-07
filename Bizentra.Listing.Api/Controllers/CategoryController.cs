using Bizentra.Listing.Application.Features.Categories.Commands.CreateCategory;
using Bizentra.Listing.Application.Features.Commands.CategoryCommand.CreateCategory;
using Bizentra.Listing.Application.Features.Commands.CategoryCommand.DeleteCategory;
using Bizentra.Listing.Application.Features.Commands.CategoryCommand.UpdateCategory;
using Bizentra.Listing.Application.Features.Queries.CategoryQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bizentra.Listing.Api.Controllers
{
    [Route("listingapi/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("id", Name = "GetParentCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetParentCategory>>> GetParentCategories()
        {
            var dtos = await _mediator.Send(new GetParentCategory.Query());
            return Ok(dtos);
        }

        [HttpGet("allsubcategories{id}", Name = "GetSubCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetSubCategoriesQuery.Result>>> GetSubCategories(Guid id)
        {
            var dtos = await _mediator.Send(new GetSubCategoriesQuery.Query() { ParentCategoryId = id });
            return Ok(dtos);
        }

        [HttpPost(Name = "AddCategory")]
        public async Task<ActionResult<List<CreateCategoryCommandResponse>>> Create([FromBody] CreateCategoryCommand categoryCommand)
        {
            var response = await _mediator.Send(categoryCommand);
            return Ok(response);
        }

        [HttpPut(Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateCategoryCommand updateCategoryCommand)
        {
            await _mediator.Send(updateCategoryCommand);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CreateCategoryCommandResponse>> DeleteCategory(Guid id)
        {
            var deleteCategory = new DeleteCategoryCommand.Command() { CategoryId = id };
            var result = await _mediator.Send(deleteCategory);

            if (!result.IsSuccessful)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
