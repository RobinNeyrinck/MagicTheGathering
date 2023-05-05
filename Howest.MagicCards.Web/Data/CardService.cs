using System.Diagnostics;
using System.Text.Json;
using Howest.MagicCards.WebAPI.Wrappers;

namespace Howest.MagicCards.Web.Data;

public class CardService
{
	private readonly ICardRepository _cardRepository;
	private readonly ICardPropertiesRepository _cardProperties;
	private readonly IMapper _mapper;
	private readonly int _maxAmount;
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonOptions;

	public CardService(ICardRepository cardRepository, IMapper mapper, IConfiguration config, ICardPropertiesRepository cardProperties, IHttpClientFactory httpClientFactory)
	{
		_cardRepository = cardRepository;
		_cardProperties = cardProperties;
		_mapper = mapper;
		_maxAmount = config.GetValue<int>("maxPageSize");
		_httpClient = httpClientFactory.CreateClient("CardsAPI");
		_jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};
	}

	public async Task<IEnumerable<CardDTO>> GetCardsAsync()
	{
		HttpResponseMessage reponse = await _httpClient.GetAsync(
			$"v1.1/Card"
			);
		string apiResponse = await reponse.Content.ReadAsStringAsync();

		if (reponse.IsSuccessStatusCode)
		{
			PagedResponse<IEnumerable<CardDTO>>? result = JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardDTO>>>(apiResponse, _jsonOptions);
			return result?.Data;
		}
		else
		{
			return new List<CardDTO>();
		}

	}

	public async Task<IEnumerable<SetDTO>> GetSetsAsync()
	{
		HttpResponseMessage response = await _httpClient.GetAsync(
			$"v1.1/Card/sets"
			);

		string apiResponse = await response.Content.ReadAsStringAsync();
		if (response.IsSuccessStatusCode)
		{
			IEnumerable<SetDTO>? result = JsonSerializer.Deserialize<IEnumerable<SetDTO>>(apiResponse, _jsonOptions);
			return result;
		}
		else
		{
			return new List<SetDTO>();
		}
	}

	public async Task<IEnumerable<TypeDTO>> GetTypesAsync()
	{
		HttpResponseMessage response = await _httpClient.GetAsync(
						$"v1.1/Card/types"
									);
		string apiResponse = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			IEnumerable<TypeDTO>? result = JsonSerializer.Deserialize<IEnumerable<TypeDTO>>(apiResponse, _jsonOptions);
			return result;
		}
		else
		{
			return new List<TypeDTO>();
		}
	}

	public async Task<IEnumerable<RarityDTO>> GetRaritiesAsync()
	{
		HttpResponseMessage response = await _httpClient.GetAsync(
									$"v1.1/Card/rarities"
																		);
		string apiResponse = await response.Content.ReadAsStringAsync();
		if (response.IsSuccessStatusCode)
		{
			IEnumerable<RarityDTO>? result = JsonSerializer.Deserialize<IEnumerable<RarityDTO>>(apiResponse, _jsonOptions);
			return result;
		}
		else
		{
			return new List<RarityDTO>();
		}
	}
}