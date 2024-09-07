using AutoMapper;
using Bizentra.Listing.Application.Persistence;
using MediatR;

namespace Bizentra.Listing.Application.Features.Queries.CategoryQuery
{
    public class GetParentCategory
    {
        public class Query : IRequest<List<Result>>
        {
            public Guid ParentCategoryId { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<Result>>
        {
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;

            public Handler(ICategoryRepository categoryRepository, IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }

            public async Task<List<Result>> Handle(Query query, CancellationToken cancellationToken)
            {
                var serviceCategories = (await _categoryRepository.GetParentCategory(query.ParentCategoryId)).OrderBy(c => c.Name);
                return _mapper.Map<List<Result>>(serviceCategories);
            }
        }
    }
}
