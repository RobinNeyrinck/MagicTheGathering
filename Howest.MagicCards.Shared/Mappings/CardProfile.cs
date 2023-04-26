namespace Shared.Mappings;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<Card, CardDTO>()
            .ForMember(dto => dto.ArtistName,
                opt => opt.MapFrom(card => card.Artist.FullName)
            )
            .ForMember(dto => dto.Description,
                opt => opt.MapFrom(card => card.Text)
            )
            .ForMember(dto => dto.Rarity,
                opt => opt.MapFrom(card => card.RarityCodeNavigation.Name)
            )
            .ForMember(dto => dto.Set,
                opt => opt.MapFrom(card => card.SetCodeNavigation.Name)
            )
            .ForMember(dto => dto.Types,
                           opt => opt.MapFrom(card => card.CardTypes.Select(ct => ct.Type.Name))
            )
            .ForMember(dto => dto.Colors,
                opt => opt.MapFrom(card => card.CardColors.Select(cc => cc.Color.Name))
            );
    }
}
