using System.Text;
using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DTO;

namespace Howest.MagicCards.WebAPI.Controllers;

[ApiVersion("1.1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CardControllerV1 : ControllerBase
{
    private const string _key = "allCards";
    private readonly ICardRepository _cardRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public CardControllerV1(ICardRepository cardRepository, IMapper mapper, IDistributedCache memoryCache)
    {
        _cardRepository = cardRepository;
        _mapper = mapper;
        _cache = memoryCache;
    }

    [HttpGet(Name = "GetCards")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<Response<IEnumerable<CardDTO>>>> GetCardsAsync([FromServices] IConfiguration config, [FromQuery] PaginationFilter paginationFilter)
    {
        string jsonData = await _cache.GetStringAsync(_key);
        IEnumerable<CardDTO>? cachedResult = (jsonData is not null)
                                            ? JsonSerializer.Deserialize<IEnumerable<CardDTO>>(jsonData)
                                            : default;

        if (cachedResult is null)
        {
            cachedResult = await _cardRepository.GetCards()
                                    .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                                    .Take(paginationFilter.PageSize)
                                    .ToListAsync();

            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };

            jsonData = JsonSerializer.Serialize(cachedResult);

            await _cache.SetStringAsync(_key, jsonData, cacheOptions);
        }

        paginationFilter.MaxPageSize = int.Parse(config["maxPageSize"]);

        return (cachedResult is IEnumerable<CardDTO> allCards)
            ? Ok(
                new PagedResponse<IEnumerable<CardDTO>>(
                            cachedResult,
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
                Message = $"No cards found"
            });
    }
}
