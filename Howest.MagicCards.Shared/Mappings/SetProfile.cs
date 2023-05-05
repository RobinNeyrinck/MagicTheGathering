namespace Howest.MagicCards.Shared.Mappings;

public class SetProfile : Profile
{
	public SetProfile()
	{
		CreateMap<Set, SetDTO>()
			.ForMember(dto => dto.Id,
			opt => opt.MapFrom(set => (int)set.Id)
		);
	}
}
