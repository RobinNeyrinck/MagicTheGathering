﻿namespace Howest.MagicCards.Shared.Mappings;

public class ArtistProfile : Profile
{
	public ArtistProfile()
	{
		CreateMap<Artist, ArtistDTO>();
	}
}
