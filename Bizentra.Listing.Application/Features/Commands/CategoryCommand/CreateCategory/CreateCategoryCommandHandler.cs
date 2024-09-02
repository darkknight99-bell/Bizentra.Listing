using AutoMapper;
using Bizentra.Listing.Application.Features.Categories.Commands.CreateCategory;
using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using MediatR;

namespace Bizentra.Listing.Application.Features.Commands.CategoryCommand.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
    {
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IBaseRepository<Category> categoryRepository, IUnitofWork unitofWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitofWork = unitofWork;
            _mapper = mapper;
        }

        public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var createCategoryCommandResponse = new CreateCategoryCommandResponse();

            var validator = new CreateCategoryCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                createCategoryCommandResponse.Success = false;
                createCategoryCommandResponse.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    createCategoryCommandResponse.ValidationErrors.Add(error.ErrorMessage);
                }
            }
            if (createCategoryCommandResponse.Success)
            {
                createCategoryCommandResponse.Message = "Category created successfully";
                var category = new Category()
                {
                    Name = request.Name,
                    ParentCategoryId = request.ParentCategoryId
                };
                category = await _categoryRepository.CreateAsync(category);
                await _unitofWork.SubmitChangesAsync();
                createCategoryCommandResponse.Category = _mapper.Map<CreateCategoryDto>(category);
            }

            return createCategoryCommandResponse;
        }
    }
}
