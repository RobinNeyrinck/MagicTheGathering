namespace Shared.Mappings;

public class SetProfile : Profile
{
    public SetProfile()
    {
        CreateMap<Set, SetDTO>();
    }
}
