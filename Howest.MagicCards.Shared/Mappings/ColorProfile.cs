namespace Shared.Mappings;

public class ColorProfile : Profile
{
    public ColorProfile()
    {
        CreateMap<Color, ColorDTO>();
    }
}
