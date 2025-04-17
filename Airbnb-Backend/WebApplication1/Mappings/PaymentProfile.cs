using AutoMapper;
using WebApplication1.DTOS.Booking;
using WebApplication1.DTOS.Payment;
using WebApplication1.Models;
using WebApplication1.Models.Enums;

namespace WebApplication1.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<CreatePaymentDTO, Payment>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}