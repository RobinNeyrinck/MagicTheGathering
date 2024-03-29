﻿namespace Howest.MagicCards.DAL.Repositories;

public interface IArtistRepository
{
    Artist GetArtist(long id);
    Task<Artist> GetArtistAsync(long id);
    Task<IEnumerable<Artist>> GetArtists();
    Task<IEnumerable<Artist>> GetLimitedArtists(int limit);
}
