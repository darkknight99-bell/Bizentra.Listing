using AutoMapper;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;
using NLog;

namespace Bizentra.Listing.Application.Features.Commands.CategoryCommand.UpdateCategory
{
    public class UpdateCategoryCommand
    {
        public class Command : IRequest<Result>
        {
            public Guid CategoryId { get; set; }
            public Guid? ParentCategoryId { get; set; }
            public string Name { get; set; } = string.Empty;
            public Category? ParentCategory { get; set; }
        }

        public class Result
        {
            public bool IsSuccessful { get; set; }
            public string Message { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IBaseRepository<Category> _categoryRepository;
            private readonly IUnitofWork _unitofWork;
            private IMapper _mapper;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public Handler(IBaseRepository<Category> categoryRepository, IUnitofWork unitofWork, IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _unitofWork = unitofWork;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = new Result
                {
                    IsSuccessful = false,
                    Message = "An error occurred while updating the category"
                };

                if (command.CategoryId == Guid.Empty && command.ParentCategoryId == Guid.Empty)
                {
                    Logger.Error($"Category Id : {command.CategoryId} and ParentCategoryId : {command.ParentCategoryId} cannot be empty");
                    return result;
                }

                var categoryToUpdate = await _categoryRepository.GetByIdAsync( command.CategoryId );
                if (categoryToUpdate == null)
                {
                    Logger.Error($"The category with Id : {command.CategoryId} cannot be found");
                    return result;
                }

                await _categoryRepository.UpdateAsync( categoryToUpdate );

                if (await _unitofWork.SubmitChangesAsync())
                {
                    result.IsSuccessful = true;
                    result.Message = ($"Category was updated successfully");
                }
                return result;
            }
        }
    }
}
