namespace Howest.MagicCards.Shared.Mappings;

public class SetProfile : Profile
{
	public SetProfile()
	{
		CreateMap<Set, SetDTO>()
			.ForMember(dto => dto.Id,
			opt => opt.MapFrom(set => (int)set.Id))
			.ForMember(dto => dto.Code,
			opt => opt.MapFrom(set => set.Code))
			.ForMember(dto => dto.Name,
			opt => opt.MapFrom(set => set.Name));
		
	}
}
