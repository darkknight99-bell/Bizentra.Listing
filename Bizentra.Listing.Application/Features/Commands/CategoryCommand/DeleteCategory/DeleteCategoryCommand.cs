using Bizentra.Listing.Application.Persistence;
using MediatR;
using Bizentra.Listing.Domain.Entities;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.CategoryCommand.DeleteCategory
{
    public class DeleteCategoryCommand
    {
        public class Command : IRequest<Result>
        {
            public Guid CategoryId { get; set; }
        }

        public class Result
        {
            public bool IsSuccessful { get; set; }
            public string Message { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IBaseRepository<Category> _categoryRepository;
            private readonly IUnitofWork _unitOfWork;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    IsSuccessful = false,
                    Message = "An errror occurred while deleting the category."
                };

                if (command.CategoryId == Guid.Empty)
                {
                    Logger.Error($"Category Id {command.CategoryId} cannot be empty");
                    return result;
                }

                var productToDelete = await _categoryRepository.GetByIdAsync(command.CategoryId);

                if (productToDelete == null)
                {
                    Logger.Error($"Category with Id : {command.CategoryId} was not found");
                    return result;
                }

                productToDelete.IsDeleted = true;

                if (await _unitOfWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = "Category deleted successfully";
                }

                return result;
            }
        }
    }
}
