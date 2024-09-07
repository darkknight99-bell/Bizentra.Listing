using AutoMapper;
using Bizentra.Listing.Application.Features.Categories.Commands.CreateCategory;
using Bizentra.Listing.Application.Features.Commands.ProductCommand.UpdateProduct;
using Bizentra.Listing.Application.Features.Commands.ServiceCommand.UpdateService;
using Bizentra.Listing.Application.Persistence.Helpers;
using Bizentra.Listing.Domain.Entities;
using static Bizentra.Listing.Application.Features.Queries.ProductQuery.GetProductsList;

namespace Bizentra.Listing.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap(typeof(Paginated<>), typeof(Paginated<>));
            CreateMap<Product, Result>()
                   .ForMember(dest => dest.Images, opts => opts.MapFrom(src => src.Images))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name));
            CreateMap<Service, Result>()
                   .ForMember(dest => dest.Images, opts => opts.MapFrom(src => src.Images))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name));

            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();
            CreateMap<Service, UpdateServiceCommand>().ReverseMap();
        }

    }
}
