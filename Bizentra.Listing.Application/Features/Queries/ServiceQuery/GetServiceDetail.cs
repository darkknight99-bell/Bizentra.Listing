using AutoMapper;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Queries.ServiceQuery
{
    public class GetServiceDetail
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Price { get; set; }
            public string? Description { get; set; }
            public ICollection<Image> Images { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Location { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? OtherInformation { get; set; }
            public Guid CategoryId { get; set; }
            public Category Category { get; set; } = default!;
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly IBaseRepository<Service> _serviceRepository;
            private readonly IMapper _mapper;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Service> serviceRepository, IMapper mapper)
            {
                _serviceRepository = serviceRepository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var service = await _serviceRepository.SingleOrDefault(x => x.Id == request.Id, ChildObjectNamesToInclude: new string[] { "Image", "Category" });

                if (service == null)
                {
                    Logger.Error($"Service with Id:{request.Id} could not be found");
                    return new Result();
                }
                var serviceDto = _mapper.Map<Result>(service);

                return serviceDto;
            }
        }
    }
}
