using AutoMapper;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.ProductCommand.UpdateProduct
{
    public class UpdateProductCommand
    {
        public class Command : IRequest<Result>
        {
            public Guid ProductId { get; set; } 
            public string Name { get; set; } = string.Empty;
            public int Price { get; set; }
            public string? Description { get; set; }
            public ICollection<Image> Images { get; set; }
            public string? PhoneNumber { get; set; }
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
            private readonly IBaseRepository<Product> _productRepository;
            private readonly IUnitofWork _unitofWork;
            private IMapper _mapper;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Product> productRepository, IUnitofWork unitofWork, IMapper mapper)
            {
                _productRepository = productRepository;
                _unitofWork = unitofWork;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    IsSuccessful = false,
                    Message = "An error occurred while updating the product"
                };

                if (command.ProductId == Guid.Empty)
                {
                    Logger.Error($"Product Id : {command.CategoryId} cannot be empty");
                    return result;
                }

                var productToUpdate = await _productRepository.GetByIdAsync(command.ProductId);
                if (productToUpdate == null)
                {
                    Logger.Error($"The product with Id : {command.ProductId} cannot be found");
                    return result;
                }

                _mapper.Map(command, productToUpdate, typeof(UpdateProductCommand), typeof(Product));

                await _productRepository.UpdateAsync(productToUpdate);

                if (await _unitofWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = ($"Product was updated successfully");
                }
                return result;
            }
        }
    }
}
