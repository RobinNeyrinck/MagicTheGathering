using AutoMapper;
using Shared.DTO;

namespace Shared.Mappings;

public class RarityProfile : Profile
{
    public RarityProfile()
    {
        CreateMap<Rarity, RarityDTO>();
    }
}
