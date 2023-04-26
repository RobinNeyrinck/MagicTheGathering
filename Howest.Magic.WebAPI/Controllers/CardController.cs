using System.Collections.Immutable;
using System.Text.Json;
using Howest.MagicCards.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

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
    [ProducesResponseType(typeof(IEnumerable<CardDTO>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<ActionResult<Response<IEnumerable<CardDTO>>>> GetCardsAsync([FromServices] IConfiguration config, [FromQuery] CardFilter filter)
    {
        string jsonData = await _cache.GetStringAsync(_key);
        IEnumerable<CardDTO>? cachedResult = (jsonData is not null)
                                            ? JsonSerializer.Deserialize<IEnumerable<CardDTO>>(jsonData)
                                            : default;

        if (cachedResult is null)
        {
            cachedResult = await _cardRepository.GetCards()
                                    .ToFilteredList(filter)
                                    .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                                    .ToPagedList(filter.PageNumber, filter.PageSize)
                                    .ToListAsync();

            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };

            jsonData = JsonSerializer.Serialize(cachedResult);

            await _cache.SetStringAsync(_key, jsonData, cacheOptions);
        }

        filter.MaxPageSize = int.Parse(config["maxPageSize"]);

        return (cachedResult is IEnumerable<CardDTO> allCards)
            ? Ok(
                new PagedResponse<IEnumerable<CardDTO>>(
                            cachedResult,
                            filter.PageNumber,
                            filter.PageSize
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

[ApiVersion("1.5")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CardControllerV2 : ControllerBase
{
    private readonly ICardRepository _cardRepository;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private const string _key = "allCards";

    public CardControllerV2(ICardRepository cardRepository, IMapper mapper, IDistributedCache memoryCache)
    {
        _cardRepository = cardRepository;
        _mapper = mapper;
        _cache = memoryCache;
    }

    [HttpGet(Name = "GetCards")]
    [ProducesResponseType(typeof(IEnumerable<CardDTO>), 200)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(typeof(string), 500)]
    public async Task<ActionResult<Response<IEnumerable<CardDTO>>>> GetCardsAsync([FromServices] IConfiguration config, [FromQuery] CardFilter filter)
    {
        //if (!filter.ValidFilters) {
        //    return BadRequest("Filters are not applicable");
        //}
        string jsonData = await _cache.GetStringAsync(_key);
        IEnumerable<CardDTO>? cachedResult = (jsonData is not null)
                                            ? JsonSerializer.Deserialize<IEnumerable<CardDTO>>(jsonData)
                                            : default;
        if (cachedResult is null)
        {
            cachedResult = await _cardRepository.GetCards()
                                    .Sort(filter.Query?? string.Empty)
                                    .ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
                                    .ToPagedList(filter.PageNumber, filter.PageSize)
                                    .ToListAsync();
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };
            jsonData = JsonSerializer.Serialize(cachedResult);
            await _cache.SetStringAsync(_key, jsonData, cacheOptions);
        }
        filter.MaxPageSize = int.Parse(config["maxPageSize"]);
        return (cachedResult is IEnumerable<CardDTO> allCards)
            ? Ok(
            new PagedResponse<IEnumerable<CardDTO>>(
                    cachedResult,
                    filter.PageNumber,
                    filter.PageSize
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
