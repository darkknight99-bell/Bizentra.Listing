using AutoMapper;
using Bizentra.Listing.Application.Features.Categories.Commands.CreateCategory;
using Bizentra.Listing.Application.Features.Commands.ProductCommand.UpdateProduct;
using Bizentra.Listing.Application.Persistence.Helpers;
using Bizentra.Listing.Domain.Entities;

namespace Bizentra.Listing.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap(typeof(Paginated<>), typeof(Paginated<>));

            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
        }

    }
}
