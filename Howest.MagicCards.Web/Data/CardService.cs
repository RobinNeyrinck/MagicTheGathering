namespace Howest.MagicCards.Web.Data;

public class CardService : ICardService
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonOptions;

	public CardService(IHttpClientFactory httpClientFactory)
	{
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

		if (reponse.IsSuccessStatusCode)
		{
			string apiResponse = await reponse.Content.ReadAsStringAsync();
			PagedResponse<IEnumerable<CardDTO>>? result = JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardDTO>>>(apiResponse, _jsonOptions);
			return result?.Data;
		}
		else
		{
			return new List<CardDTO>();
		}
	}

	public async Task<CardDetailDTO> GetCardByIdAsync(long id)
	{
		HttpResponseMessage response = await _httpClient.GetAsync(
	$"v1.5/Card/{id}"
		);

		if (response.IsSuccessStatusCode)
		{
			string apiResponse = await response.Content.ReadAsStringAsync();
			CardDetailDTO? result = JsonSerializer.Deserialize<CardDetailDTO>(apiResponse, _jsonOptions);
			return result;
		}
		else
		{
			return new CardDetailDTO();
		}
	}

	public async Task<IEnumerable<SetDTO>> GetSetsAsync()
	{
		HttpResponseMessage response = await _httpClient.GetAsync(
			$"v1.1/Card/sets"
			);

		if (response.IsSuccessStatusCode)
		{
			string apiResponse = await response.Content.ReadAsStringAsync();
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

		if (response.IsSuccessStatusCode)
		{
			string apiResponse = await response.Content.ReadAsStringAsync();
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
		if (response.IsSuccessStatusCode)
		{
			string apiResponse = await response.Content.ReadAsStringAsync();
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
		Dictionary<string, string> queryParams = new()
		{
			["Name"] = filter.Name,
			["Text"] = filter.Text,
			["Set"] = filter.Set,
			["Rarity"] = filter.Rarity,
			["type"] = filter.Type
		};

		string queryString = string.Join("&", queryParams
			.Where(kv => !string.IsNullOrEmpty(kv.Value))
			.Select(kv => $"{kv.Key}={kv.Value}"));

		string apiUrl = $"v1.1/Card?{queryString}";

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

	public async Task<IEnumerable<CardDTO>>? Sort(bool ascending)
	{
		string direction;
		if (ascending)
		{
			direction = "asc";
		}
		else
		{
			direction = "desc";
		}

		string apiUrl = $"v1.5/Card?query=name {direction}";

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
}