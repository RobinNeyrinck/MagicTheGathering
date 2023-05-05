namespace Howest.MagicCards.Shared.Mappings;

public class TypeProfile : Profile
{
	public TypeProfile()
	{
		CreateMap<DAL.Models.Type, TypeDTO>()
			.ForMember(dto => dto.Id,
			opt => opt.MapFrom(rarity => (int)rarity.Id));

	}

}
