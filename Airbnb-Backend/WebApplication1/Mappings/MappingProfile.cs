using AutoMapper;
using WebApplication1.DTOS.Listing;
using WebApplication1.Models;

namespace WebApplication1.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<UpdateListingDTO, Listing>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateListingDTO, Listing>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Listing, GetListingDTO>()
                .ForMember(dest => dest.ImageUrls,
                    opt => opt.MapFrom(src => src.ListingPhotos.Select(p => p.Url).ToList()))
                .ForMember(dest => dest.PreviewImageUrl,
                           opt => opt.MapFrom(src => src.ListingPhotos
                                                    .Where(p => p.IsPrimary == true)
                                                    .Select(p => p.Url)
                                                    .FirstOrDefault()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}