using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe;
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
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Payment, PaymentResponseDTO>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Name))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Code))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<(PaymentIntent intent, Charge charge, ConfirmPaymentDTO dto), CreatePaymentDTO>()
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.charge.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => (decimal)src.intent.Amount / 100))
                .ForMember(dest => dest.ReceiptUrl, opt => opt.MapFrom(src => src.charge.ReceiptUrl))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ParsePaymentStatus(src.intent.Status)))
                .ForMember(dest => dest.FailureMessage, opt => opt.MapFrom(src => src.charge.FailureMessage))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.ProccessedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }

        private static PaymentStatus ParsePaymentStatus(string stripeStatus)
        {
            return stripeStatus.ToLower() switch
            {
                "succeeded" => PaymentStatus.Completed,
                "processing" => PaymentStatus.Pending,
                "requires_payment_method" => PaymentStatus.Pending,
                "requires_confirmation" => PaymentStatus.Pending,
                "canceled" => PaymentStatus.Refunded,
                _ => PaymentStatus.Failed
            };
        }

    }
}