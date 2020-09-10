using System;
using System.Linq;
using Authorization.DTO;
using AutoMapper;
using IdentityServer4.Models;

namespace Authorization.Resources.Api
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(config =>
            {
                // IDENTITY USER ->USER DTO
                config.CreateMap<User, UserDTO>()
                    .ReverseMap();

                // TENANT -> TENANT DTO
                config.CreateMap<Tenant, TenantDTO>()
                      .ReverseMap();

                // CLIENT -> CLIENT DTO
                config.CreateMap<Client, ClientDTO>()
                .ForMember(m => m.ClientSecret, obj => obj.MapFrom(src => src.ClientSecrets.FirstOrDefault().Value))
                .ForMember(m => m.TenantId, obj => obj.MapFrom(src => src.Properties["TenantId"]))
                      .ReverseMap();

                // APIRESOURCE -> APIRESOURCE DTO
                config.CreateMap<ApiResource, ApiResourceDTO>()
                    .ReverseMap();
            });
        }
    }
}
