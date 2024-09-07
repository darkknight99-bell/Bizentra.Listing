using AutoMapper;
using Bizentra.Listing.Application.Persistence.Helpers;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Queries.ServiceQuery
{
    public class GetServiceList
    {
        public class Query : IRequest<Paginated<Result>>
        {
            public Guid CategoryId { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public int? Page { get; set; } = 1;
            public int? PageSize { get; set; } = 50;
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public Guid CategoryId { get; set; }
            public string CategoryName { get; set; }
            public int Price { get; set; }
            public string? PhoneNumber { get; set; }

            public ICollection<Image> Images { get; set; }
        }

        public class Handler : IRequestHandler<Query, Paginated<Result>>
        {
            private readonly IBaseRepository<Service> _serviceRepository;
            private readonly IMapper _mapper;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Service> serviceRepository, IMapper mapper)
            {
                _serviceRepository = serviceRepository;
                _mapper = mapper;
            }

            public async Task<Paginated<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var services = await _serviceRepository.GetWherePaginated(new PaginatedQuery<Service>
                {
                    predicate = x => x.CategoryId == request.CategoryId && string.IsNullOrEmpty(request.City) || x.City.Contains(request.City)
                                    && string.IsNullOrEmpty(request.State) || x.State.ToLower() == request.State.ToLower(),
                    ChildObjectNamesToInclude = new string[] { "Image", "Category" },
                    PageSize = request.PageSize.Value,
                    Page = request.Page.Value
                });
                if(services == null)
                {
                    Logger.Error($"No service found for this category");
                }

                return _mapper.Map<Paginated<Result>>(services);
            }
        }
    }
}
