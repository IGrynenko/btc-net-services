using AutoMapper;
using UserService.Models;
using System;

namespace UserService.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(e => Guid.NewGuid()))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(e => DateTime.Now));

            CreateMap<User, UserModel>();

            CreateMap<User, UserDTO>();
        }
    }
}
