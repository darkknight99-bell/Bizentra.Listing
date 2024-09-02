using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.ProductCommand.DeleteProduct
{
    public class DeleteProductCommand
    {
        public class Command : IRequest<Result>
        {
            public Guid ProductId { get; set; }
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
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    IsSuccessful = false,
                    Message = ""
                };

                if (command.ProductId == Guid.Empty)
                {
                    Logger.Error("Product Id cannot be empty");
                    return result;
                }

                var productToDelete = await _productRepository.GetByIdAsync(command.ProductId);
                if (productToDelete == null)
                {
                    Logger.Error($"Category with Id : {command.ProductId} was not found");
                    return result;
                }

                productToDelete.IsDeleted = true;

                if (await _unitofWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = "Product deleted successfully";
                }

                return result;
            }
        }
    }
}
