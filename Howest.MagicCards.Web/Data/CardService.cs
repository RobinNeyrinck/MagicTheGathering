using System.Text.Json;
using System.Web;
using Howest.MagicCards.WebAPI.Wrappers;

namespace Howest.MagicCards.Web.Data;

public class CardService
{
	private readonly ILogger _logger = new Logger<CardService>(new LoggerFactory());
	private readonly int _maxAmount;
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonOptions;

	public CardService(IMapper mapper, IConfiguration config, IHttpClientFactory httpClientFactory)
	{
		_maxAmount = config.GetValue<int>("maxPageSize");
		_httpClient = httpClientFactory.CreateClient("CardsAPI");
		_jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};
	}

	#region Data Loading
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
	#endregion

	#region Data Manipulation
	public async Task<IEnumerable<CardDTO>> Filter(CardFilterArgs filter)
	{
		var queryParams = new Dictionary<string, string>
		{
			["Name"] = filter.Name,
			["Text"] = filter.Text,
			["Set"] = filter.Set,
			["Rarity"] = filter.Rarity,
			//["type"] = filter.Type
		};

		string queryString = string.Join("&", queryParams
			.Where(kv => !string.IsNullOrEmpty(kv.Value))
			.Select(kv => $"{kv.Key}={HttpUtility.UrlEncode(kv.Value)}"));

		string apiUrl = $"v1.1/Card?{queryString}";

		_logger.LogInformation($"API URL: {apiUrl}");

		HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

		if (response.IsSuccessStatusCode)
		{
			string apiResponse = await response.Content.ReadAsStringAsync();
			PagedResponse<IEnumerable<CardDTO>> result = JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardDTO>>>(apiResponse, _jsonOptions);
			return result?.Data;
		}
		else
		{
			return new List<CardDTO>();
		}
	}

	#endregion

	public class CardFilterArgs
	{
		public string Name { get; set; }
		public string Text { get; set; }
		public string Set { get; set; }
		public string Rarity { get; set; }
		public string Type { get; set; }
	}
}