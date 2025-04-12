using AutoMapper;
using WebApplication1.DTOS.Amenity;
using WebApplication1.DTOS.Listing;
using WebApplication1.DTOS.Review;
using WebApplication1.Models;

namespace WebApplication1.Mappings
{
    public class ListingProfile : Profile
    {
        public ListingProfile()
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
                .ForMember(dest => dest.Amenities,
                           opt => opt.MapFrom(src => src.ListingAmenities.Select(la => new GetAmenityDTO
                           {
                               Id = la.Amenity.Id,
                               Name = la.Amenity.Name,
                               Icon = la.Amenity.Icon,
                               CategoryId = la.Amenity.CategoryId
                           }).ToList()))
                .ForMember(dest => dest.Reviews,
                            opt => opt.MapFrom(src => src.Reviews.Select(r => new GetReviewDTO
                            {
                                Id = r.Id,
                                Comment = r.Comment,
                                Rating = r.Rating,
                                CreatedAt = r.CreatedAt,
                                HostId = r.HostId
                            }).ToList()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Amenity, GetAmenityDTO>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Review, GetReviewDTO>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}