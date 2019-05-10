using AutoMapper;
using document_server2.Core.Domain;
using document_server2.Infrastructure.DTO;
using static document_server2.Infrastructure.Comends.CreateCase;
using static document_server2.Infrastructure.DTO.CaseDetailsDTO;

namespace document_server2.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<Document, DocumentDTO>();
            CreateMap<Case, CaseDTO>();
            CreateMap<Case, CaseDetailsDTO>();
            CreateMap<SendDocument, Document>();
        }
    }
}
