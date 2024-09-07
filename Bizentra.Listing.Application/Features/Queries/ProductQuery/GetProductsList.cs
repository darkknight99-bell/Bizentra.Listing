using AutoMapper;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Application.Persistence.Helpers;
using Bizentra.Listing.Domain.Entities;
using MediatR;
 
namespace Bizentra.Listing.Application.Features.Queries.ProductQuery
{
    public class GetProductsList
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
            private readonly IBaseRepository<Product> _productRepository;
            private readonly IMapper _mapper;

            public Handler(IBaseRepository<Product> productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<Paginated<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _productRepository.GetWherePaginated(new PaginatedQuery<Product>
                {
                    predicate = x => x.CategoryId == request.CategoryId && string.IsNullOrEmpty(request.City) || x.City.Contains(request.City)
                                    && string.IsNullOrEmpty(request.State) || x.State.ToLower() == request.State.ToLower(),
                    ChildObjectNamesToInclude = new string[] { "Image", "Category" },
                    PageSize = request.PageSize.Value,
                    Page = request.Page.Value
                });
                return _mapper.Map<Paginated<Result>>(products);
            }
        }
    }
}
