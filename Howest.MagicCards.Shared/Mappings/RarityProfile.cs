namespace Howest.MagicCards.Shared.Mappings;

public class RarityProfile : Profile
{
	public RarityProfile()
	{
		CreateMap<Rarity, RarityDTO>()
			.ForMember(dto => dto.Id,
			opt => opt.MapFrom(rarity => (int)rarity.Id));
	}
}
