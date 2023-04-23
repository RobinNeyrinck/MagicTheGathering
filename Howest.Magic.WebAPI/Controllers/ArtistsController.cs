using Howest.MagicCards.Shared.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ImtgRepository _mtgRepository;

        public ArtistsController(ImtgRepository mtgRepository)
        {
            _mtgRepository = mtgRepository;

        }

        [HttpGet]
        public IActionResult GetAllArtists([FromQuery] PaginationFilter paginationFilter, [FromServices] IConfiguration config)
        {
            paginationFilter.MaxPageSize = int.Parse(config["maxPageSize"]);
            return (_mtgRepository.GetArtists() is IEnumerable<Artist> allArtists)
                    ? Ok(new PagedResponse<IEnumerable<Artist>>(
                            allArtists
                                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                                .Take(paginationFilter.PageSize)
                                .ToList(),
                            paginationFilter.PageNumber,
                            paginationFilter.PageSize
                        )
                    {
                        TotalRecords = allArtists.Count()
                    })
                    : NotFound("No artists found");
        }
    }
}
