using AutoMapper;
using document_server2.Core.Domain;
using document_server2.Infrastructure.DTO;

namespace document_server2.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
