using Bizentra.Listing.Api.ApiResources;
using Bizentra.Listing.Application.Features.Commands.ServiceCommand.AddService;
using Bizentra.Listing.Application.Features.Commands.ServiceCommand.DeleteService;
using Bizentra.Listing.Application.Features.Commands.ServiceCommand.UpdateService;
using Bizentra.Listing.Application.Features.Queries.ServiceQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bizentra.Listing.Api.Controllers
{
    [Route("listingapi/[controller]")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly IMediator _mediator;

        public ServiceController(IMediator mediator, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _mediator = mediator;
        }


        [HttpGet("all", Name = "GetAllServices")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetServiceList.Result>>> GetAllServices([FromQuery] SearchQuery query)
        {
            var dto = await _mediator.Send(new GetServiceList.Query
            {
                Page = query.Page,
                State = query.State,
                City = query.City,
                PageSize = query.PageSize,
            });
            return Ok(dto);
        }
        
        [HttpGet("{id}", Name = "GetServiceById")]
        public async Task<ActionResult<List<GetServiceDetail.Result>>> GetServiceById(Guid id)
        {
            var result = await _mediator.Send(new GetServiceDetail.Query() { Id = id });
            return Ok(result);
        }

        [Authorize]
        [HttpPost(Name = "AddService")]
        public async Task<ActionResult<Guid>> Create([FromBody] AddServiceCommand addServiceCommand)
        {
            var id = await _mediator.Send(addServiceCommand);
            return Ok(id);
        }

        [Authorize]
        [HttpPut(Name = "UpdateService")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateServiceCommand updateServiceCommand)
        {
            await _mediator.Send(updateServiceCommand);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}", Name = "DeleteService")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleteService = new DeleteServiceCommand.Command() { ServiceId = id };
            await _mediator.Send(deleteService);
            return NoContent();
        }
    }
}
