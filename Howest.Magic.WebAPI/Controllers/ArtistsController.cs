using Microsoft.AspNetCore.Mvc;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiVersion("1.1")]
    [ApiController]
    public class ArtistsControllerV1 : ControllerBase
    {
        private readonly IArtistRepository _mtgRepository;

        public ArtistsControllerV1(IArtistRepository mtgRepository)
        {
            _mtgRepository = mtgRepository;

        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Response<IEnumerable<Artist>>> GetAll([FromQuery] PaginationFilter paginationFilter, [FromServices] IConfiguration config)
        {
            paginationFilter.MaxPageSize = int.Parse(config["maxPageSize"]);
            try
            {
                return (_mtgRepository.GetArtists() is IQueryable<Artist> allArtists)
                    ? Ok(
                    new PagedResponse<IEnumerable<Artist>>(
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
                    : NotFound(new Response<Artist>
                    {
                        Succeeded = false,
                        Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}" },
                        Message = $"No books found"
                    });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response<Artist>()
                    {
                        Succeeded = false,
                        Errors = new[] { $"Status code: {StatusCodes.Status500InternalServerError}" },
                        Message = $"{ex.Message}"
                    });
            }

        }
    }
}
