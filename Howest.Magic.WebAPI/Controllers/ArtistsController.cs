using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ImtgRepository _mtgRepository;

        public ArtistsController(ImtgRepository mtgRepository) { 
            _mtgRepository = mtgRepository;

        }

        [HttpGet]
        public IActionResult GetAllArtists() {
            return (_mtgRepository.GetArtists() is IEnumerable<Artist> allArtists)
                    ? Ok(allArtists)
                    : NotFound("No artists found");
        }
    }
}
