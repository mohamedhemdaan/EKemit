using AutoMapper;
using EKemit.APIs.DTOs;
using EKemit.APIs.DTOs.Dashbord_DTOs;
using EKemit.Core.Entities;
using EKemit.Core.Entities.Order_Aggregate;

namespace EKemit.APIs.Helpers
{
    public class GovernratesPictureUrlResolver : IValueResolver<Governrate,GovernrateDashToReturnDto,string?>
    {
        private readonly IConfiguration _configuration;

        public GovernratesPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Governrate source, GovernrateDashToReturnDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
