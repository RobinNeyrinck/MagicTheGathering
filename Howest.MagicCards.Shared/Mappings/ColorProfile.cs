using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mappings;

public class ColorProfile : Profile
{
	public ColorProfile()
	{
		CreateMap<Color, ColorDTO>();
	}
}
