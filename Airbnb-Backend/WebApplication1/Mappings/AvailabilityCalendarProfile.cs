using AutoMapper;
using WebApplication1.DTOS.AvailabilityCalendar;
using WebApplication1.Models;

namespace WebApplication1.Mappings
{
    public class AvailabilityCalendarProfile : Profile
    {
        public AvailabilityCalendarProfile()
        {
            CreateMap<AvailabilityCalendar, GetAvailabilityCalendarDTO>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<InitAvailabilityCalendarDTO, AvailabilityCalendar>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => true))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<SetAvailabilityCalendarDTO, AvailabilityCalendar>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.StartDate))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
