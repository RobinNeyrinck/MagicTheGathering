﻿using System.Text.Json;
using Howest.MagicCards.Shared.DTO;
using Microsoft.Extensions.Caching.Distributed;

namespace Howest.MagicCards.WebAPI.Controllers;

[ApiVersion("1.1")]
[ApiVersion("1.5")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CardController : ControllerBase
{
	private const string _key = "allCards";
	private readonly ICardRepository _cardRepository;
	private readonly IMapper _mapper;
	private readonly IDistributedCache _cache;
	private readonly ICardPropertiesRepository _cardPropertiesRepository;

	public CardController(ICardRepository cardRepository, IMapper mapper, IDistributedCache memoryCache, ICardPropertiesRepository cardPropertiesRepository)
	{
		_cardRepository = cardRepository;
		_mapper = mapper;
		_cache = memoryCache;
		_cardPropertiesRepository = cardPropertiesRepository;
	}

	#region v1.1
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<CardDTO>), 200)]
	[ProducesResponseType(typeof(string), 404)]
	[ProducesResponseType(typeof(string), 500)]
	[MapToApiVersion("1.1")]
	public async Task<ActionResult<Response<IEnumerable<CardDTO>>>> GetCardsAsync([FromServices] IConfiguration config, [FromQuery] CardFilter filter)
	{
		filter.MaxPageSize = int.Parse(config["maxPageSize"]);
		if (filter.PageSize == 1)
		{
			filter.PageSize = filter.MaxPageSize;
		}


		int skipAmount = (filter.PageNumber - 1) * filter.PageSize;
		int totalRecords = await _cardRepository.GetCards().CountAsync();
		int totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);
		string jsonData = await _cache.GetStringAsync(_key);
		IEnumerable<CardDTO>? cachedResult = (jsonData is not null)
											? JsonSerializer.Deserialize<IEnumerable<CardDTO>>(jsonData)
											: default;

		if (cachedResult is null)
		{
			cachedResult = await _cardRepository.GetCards()
									.ToFilteredList(filter)
									.Skip(skipAmount)
									.Take(filter.PageSize)
									.ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
									.ToListAsync();

			DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
			};

			jsonData = JsonSerializer.Serialize(cachedResult);

			await _cache.SetStringAsync(_key, jsonData, cacheOptions);
		}

		return (cachedResult is IEnumerable<CardDTO> allCards)
			? Ok(
				new PagedResponse<IEnumerable<CardDTO>>(
							allCards,
							filter.PageNumber,
							filter.PageSize
					)
				{
					TotalRecords = totalRecords,
					TotalPages = totalPages

				})
			: NotFound(new Response<CardDTO>
			{
				Succeeded = false,
				Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}" },
				Message = $"No cards found"
			});
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(CardDTO), 200)]
	[ProducesResponseType(typeof(string), 404)]
	[ProducesResponseType(typeof(string), 500)]
	[MapToApiVersion("1.1")]
	public async Task<ActionResult<Response<CardDTO>>> GetCardByIdAsync(int id)
	{
		try
		{
			return (_cardRepository.GetCardById(id) is IQueryable<Card> card)
				? Ok(await card
							.ProjectTo<CardDTO>(_mapper.ConfigurationProvider)
							.FirstOrDefaultAsync())
				: NotFound(new Response<CardDTO>
				{
					Succeeded = false,
					Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}" },
					Message = $"No card found with id: {id}"
				});
		}
		catch (Exception ex)
		{
			return StatusCode(
							   StatusCodes.Status500InternalServerError,
											  new Response<CardDTO>
											  {
												  Succeeded = false,
												  Errors = new[] { ex.Message },
												  Message = $"Error while retrieving card with id: {id}"
											  });
		}
	}

	#region Card Properties
	[HttpGet("colors")]
	[ProducesResponseType(typeof(IEnumerable<ColorDTO>), 200)]
	[ProducesResponseType(typeof(string), 404)]
	[ProducesResponseType(typeof(string), 500)]
	[MapToApiVersion("1.1")]
	public async Task<ActionResult<Response<IEnumerable<ColorDTO>>>> GetColors()
	{
		try
		{
			return (_cardPropertiesRepository.GetColors() is IQueryable<Color> allColors)
				? Ok(await allColors
						.ProjectTo<ColorDTO>(_mapper.ConfigurationProvider)
						.ToListAsync())
				: NotFound(new Response<Color>
				{
					Succeeded = false,
					Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}" },
					Message = $"No colors found"
				});
		}
		catch (Exception ex)
		{
			return StatusCode(
				StatusCodes.Status500InternalServerError,
				new Response<ColorDTO>
				{
					Succeeded = false,
					Errors = new[] { ex.Message },
					Message = $"Error while retrieving colors"
				});
		}
	}

	[HttpGet("rarities")]
	[ProducesResponseType(typeof(IEnumerable<RarityDTO>), 200)]
	[ProducesResponseType(typeof(string), 404)]
	[ProducesResponseType(typeof(string), 500)]
	[MapToApiVersion("1.1")]
	public async Task<ActionResult<Response<IEnumerable<RarityDTO>>>> GetRarities()
	{
		try
		{
			return (_cardPropertiesRepository.GetRarities() is IQueryable<Rarity> allRarities)
				? Ok(await allRarities
									   .ProjectTo<RarityDTO>(_mapper.ConfigurationProvider)
															  .ToListAsync())
				: NotFound(new Response<RarityDTO>
				{
					Succeeded = false,
					Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}" },
					Message = $"No rarities found"
				});
		}
		catch (Exception ex)
		{
			return StatusCode(
							   StatusCodes.Status500InternalServerError,
											  new Response<RarityDTO>
											  {
												  Succeeded = false,
												  Errors = new[] { ex.Message },
												  Message = $"Error while retrieving rarities"
											  });
		}
	}

	[HttpGet("sets")]
	[ProducesResponseType(typeof(IEnumerable<SetDTO>), 200)]
	[ProducesResponseType(typeof(string), 404)]
	[ProducesResponseType(typeof(string), 500)]
	[MapToApiVersion("1.1")]
	public async Task<ActionResult<Response<IEnumerable<SetDTO>>>> GetSets()
	{
		try
		{
			return (_cardPropertiesRepository.GetSets() is IQueryable<Set> allSets)
				? Ok(await allSets
						.ProjectTo<SetDTO>(_mapper.ConfigurationProvider)
						.ToListAsync())
				: NotFound(new Response<SetDTO>
				{
					Succeeded = false,
					Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}" },
					Message = $"No sets found"
				});
		}
		catch (Exception ex)
		{
			return StatusCode(
				StatusCodes.Status500InternalServerError,
			new Response<SetDTO>
			{
				Succeeded = false,
				Errors = new[] { ex.Message },
				Message = $"Error while retrieving sets"
			});
		}
	}

	#endregion
	#endregion

	#region v1.5
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<CardDTO>), 200)]
	[ProducesResponseType(typeof(string), 404)]
	[ProducesResponseType(typeof(string), 500)]
	[MapToApiVersion("1.5")]
	public async Task<ActionResult<Response<IEnumerable<CardDTO>>>> GetDetailedCardsAsync([FromServices] IConfiguration config, [FromQuery] CardFilter filter)
	{
		//if (!filter.ValidFilters) {
		//    return BadRequest("Filters are not applicable");
		//}
		string jsonData = await _cache.GetStringAsync(_key);
		IEnumerable<CardDetailDTO>? cachedResult = (jsonData is not null)
											? JsonSerializer.Deserialize<IEnumerable<CardDetailDTO>>(jsonData)
											: default;
		if (cachedResult is null)
		{
			cachedResult = await _cardRepository.GetCards()
									.Sort(filter.Query ?? string.Empty)
									.ProjectTo<CardDetailDTO>(_mapper.ConfigurationProvider)
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
		return (cachedResult is IEnumerable<CardDetailDTO> allCards)
			? Ok(
			new PagedResponse<IEnumerable<CardDetailDTO>>(
					cachedResult,
					filter.PageNumber,
					filter.PageSize
				  )
			{
				TotalRecords = allCards.Count()
			})
			: NotFound(new Response<CardDetailDTO>
			{
				Succeeded = false,
				Errors = new[] { $"Status code: {StatusCodes.Status404NotFound}" },
				Message = $"No cards found"
			});
	}
	#endregion

}
