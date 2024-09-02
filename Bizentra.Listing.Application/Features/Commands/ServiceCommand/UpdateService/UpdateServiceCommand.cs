using AutoMapper;
using Bizentra.Listing.Application.Features.Commands.ProductCommand.UpdateProduct;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.ServiceCommand.UpdateService
{
    public class UpdateServiceCommand
    {
        public class Command : IRequest<Result>
        {
            public Guid ServiceId { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Price { get; set; }
            public string? Description { get; set; }
            public ICollection<Image> Images { get; set; }
            public string? Business { get; set; }
            public string? Location { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class Result
        {
            public bool IsSuccessful { get; set; }
            public string Message { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IBaseRepository<Service> _serviceRepository;
            private readonly IUnitofWork _unitofWork;
            private IMapper _mapper;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Service> serviceRepository, IUnitofWork unitofWork, IMapper mapper)
            {
                _serviceRepository = serviceRepository;
                _unitofWork = unitofWork;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    IsSuccessful = false,
                    Message = "An error occurred while updating the service"
                };

                if (command.ServiceId == Guid.Empty)
                {
                    Logger.Error($"Service Id : {command.CategoryId} cannot be empty");
                    return result;
                }

                var serviceToUpdate = await _serviceRepository.GetByIdAsync(command.ServiceId);
                if (serviceToUpdate == null)
                {
                    Logger.Error($"The service with Id : {command.ServiceId} cannot be found");
                    return result;
                }

                _mapper.Map(command, serviceToUpdate, typeof(UpdateProductCommand), typeof(Product));

                await _serviceRepository.UpdateAsync(serviceToUpdate);

                if (await _unitofWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = ($"Service was updated successfully");
                }
                return result;
            }
        }
    }
}
