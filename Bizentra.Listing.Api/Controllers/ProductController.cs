using Bizentra.Listing.Api.ApiResources;
using Bizentra.Listing.Application.Features.Commands.ProductCommand.AddProduct;
using Bizentra.Listing.Application.Features.Commands.ProductCommand.DeleteProduct;
using Bizentra.Listing.Application.Features.Commands.ProductCommand.UpdateProduct;
using Bizentra.Listing.Application.Features.Queries.ProductQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizentra.Listing.Api.Controllers
{
    [Route("listingapi/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _mediator = mediator;
        }


        [HttpGet("all", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetProductsList.Result>>> GetProductList([FromQuery] SearchQuery query)
        {
            var dto = await _mediator.Send(new GetProductsList.Query
            {
                Page = query.Page,
                State = query.State,
                City = query.City,
                PageSize = query.PageSize,
            });
            return Ok(dto);
        }

        [HttpGet("{id}", Name = "GetServiceById")]
        public async Task<ActionResult<List<GetProductDetail.Result>>> GetProductById(Guid id)
        {
            var result = await _mediator.Send(new GetProductDetail.Query() { Id = id });
            return Ok(result);
        }

        [Authorize]
        [HttpPost(Name = "AddProduct")]
        public async Task<ActionResult<Guid>> Create([FromBody] AddProductCommand addProductCommand)
        {
            var id = await _mediator.Send(addProductCommand);
            return Ok(id);
        }

        [Authorize]
        [HttpPut(Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateProductCommand updateProductCommand)
        {
            await _mediator.Send(updateProductCommand);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleteProduct= new DeleteProductCommand.Command() { ProductId = id };
            await _mediator.Send(deleteProduct);
            return NoContent();
        }
    }
}
