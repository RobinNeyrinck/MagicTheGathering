namespace Howest.MagicCards.Shared.Mappings;

public class TypeProfile : Profile
{
	public TypeProfile()
	{
		CreateMap<DAL.Models.Type, TypeDTO>()
			.ForMember(dto => dto.Id,
			opt => opt.MapFrom(type => (int)type.Id))
			.ForMember(dto => dto.Type,
			opt => opt.MapFrom(type => type.Type1));

	}

}
