using AutoMapper;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;
using static URLShortener.BLL.Components.SystemClaims.Permissions;

namespace URLShortener.BLL.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<ApplicationUser, UserSimpleDTO>();

            CreateMap<UserDTO, ApplicationUser>();
            CreateMap<UserInput, ApplicationUser>();

            CreateMap<ApplicationUserRole, RoleDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Role.NormalizedName));

            CreateMap<ApplicationRole, RoleDTO>();
            CreateMap<RoleDTO, ApplicationRole>();
            CreateMap<Claim, ClaimDTO>();


            // make sure all datetime values are UTC
            ValueTransformers.Add<DateTime>(val => !((DateTime?)val).HasValue ? val : DateTime.SpecifyKind(val, DateTimeKind.Utc));
            ValueTransformers.Add<DateTime?>(val => val.HasValue ? DateTime.SpecifyKind(val.Value, DateTimeKind.Utc) : val);
        }
    }
}
