using AutoMapper;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.ProductCommand.AddProduct
{
    public class AddProductCommand
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
            private readonly IBaseRepository<Product> _productRepository;
            private readonly IMapper _mapper;
            private readonly IUnitofWork _unitofWork;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Product> productRepository, IMapper mapper, IUnitofWork unitofWork)
            {
                _productRepository = productRepository;
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

                var product = _mapper.Map<Product>(command);
                product = await _productRepository.CreateAsync(product);

                if(await _unitofWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = ($"Product with Id : {product.Id} has been added successfully");
                    Logger.Info($"Product {product.Id} created successfully");
                }

                return result;
            }
        }
    }
}
