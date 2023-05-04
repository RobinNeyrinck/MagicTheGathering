using Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.WebAPI.Controllers;

[ApiVersion("1.1")]
[ApiVersion("1.5")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly IArtistRepository _artistRepository;
    private readonly IMapper _mapper;

    public ArtistController(IArtistRepository artistRepository, IMapper mapper)
    {
        _artistRepository = artistRepository;
        _mapper = mapper;
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ArtistDTO), 200)]
    [ProducesResponseType(typeof(Response<ArtistDTO>), 404)]
    [ProducesResponseType(typeof(Response<ArtistDTO>), 500)]
    public async Task<ActionResult<Artist>> GetArtist(long id)
    {
        return (await _artistRepository.GetArtistAsync(id) is Artist artist)
            ? Ok(_mapper.Map<ArtistDTO>(artist))
            : NotFound(new Response<Artist>
            {
                Succeeded = false,
                Message = $"Artist with id {id} not found",
                Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}"}
            });
    }
}
