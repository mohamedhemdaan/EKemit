using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.Core.Entities;
using EKemit.Core.Entities.Basket;
using EKemit.Core.Entities.Identity;
using EKemit.Core.Entities.Order_Aggregate;
using OrderAddress = EKemit.Core.Entities.Order_Aggregate.Address;
using IdentityAddress = EKemit.Core.Entities.Identity.Address;
using EKemit.APIs.DTOs.Dashbord_DTOs;

namespace EKemit.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                     .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                     .ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                     .ForMember(d => d.Governrate, O => O.MapFrom(S => S.Governrate.Name))
                     .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>())
                     .ForMember(d => d.comments, o => o.MapFrom(s => s.Comments.Select(c => c.Text).ToList()));  // Map comments to their texts



            CreateMap<Product, ProductDashDto>().ReverseMap();
            CreateMap<BrandOrTypeDto, ProductBrand>();
            CreateMap<BrandOrTypeDto, ProductType>();

            CreateMap<Governrate, GovernrateDashToReturnDto>()
                .ForMember(d => d.PictureUrl, O => O.MapFrom<GovernratesPictureUrlResolver>())
                .ReverseMap();

            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap<AddressDto, OrderAddress>();

            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                     .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                     .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                     .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                     .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                     .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                     .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());



        }
    }
}
