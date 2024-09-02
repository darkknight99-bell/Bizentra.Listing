using AutoMapper;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.ServiceCommand.AddService
{
    public class AddServiceCommand
    {
        public class Command : IRequest<Result>
        {
            public string Name { get; set; } = string.Empty;
            public int Price { get; set; }
            public string? Description { get; set; }
            public ICollection<Image> Images { get; set; }
            public string? Business { get; set; }
            public string? Location { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public Guid CategoryId { get; set; }
            public bool IsDeleted { get; set; } = false;
        }

        public class Result
        {
            public bool IsSuccessful { get; set; }
            public string Message { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IBaseRepository<Service> _serviceRepository;
            private readonly IMapper _mapper;
            private readonly IUnitofWork _unitofWork;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Service> serviceRepository, IMapper mapper, IUnitofWork unitofWork)
            {
                _serviceRepository = serviceRepository;
                _mapper = mapper;
                _unitofWork = unitofWork;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    IsSuccessful = false,
                    Message = ""
                };

                var service = _mapper.Map<Service>(command);
                service = await _serviceRepository.CreateAsync(service);

                if (await _unitofWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = ($"Product with Id : {service.Id} has been added successfully");
                    Logger.Info($"Product {service.Id} created successfully");
                }

                return result;
            }
        }
    }
}
