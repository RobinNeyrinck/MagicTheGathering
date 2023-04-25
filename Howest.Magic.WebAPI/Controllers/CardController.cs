using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace Howest.MagicCards.WebAPI.Controllers
{
    [ApiVersion("1.1")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class CardControllerV1 : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardControllerV1(ICardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Response<IEnumerable<CardDTO>>> Get([FromServices] IConfiguration config, [FromQuery] PaginationFilter paginationFilter)
        {
            paginationFilter.MaxPageSize = int.Parse(config["maxPageSize"]);
            try
            {
                return (_cardRepository.GetCards() is IQueryable<Card> allCards)
                    ? Ok(
                        new PagedResponse<IEnumerable<CardDTO>>(
                                    allCards
                                        .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                                        .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                                        .Take(paginationFilter.PageSize)
                                        .ToList(),
                                    paginationFilter.PageNumber,
                                    paginationFilter.PageSize
                            )
                        {
                            TotalRecords = allCards.Count()
                        })
                    : NotFound(new Response<CardDTO>
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
                        new Response<Card>()
                        {
                            Succeeded = false,
                            Errors = new[] { $"Status code: {StatusCodes.Status500InternalServerError}" },
                            Message = $"{ex.Message}"
                        });
            }

        }
    }
}
