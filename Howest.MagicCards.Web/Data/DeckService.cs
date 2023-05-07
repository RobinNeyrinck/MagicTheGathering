using System.Text.Json;

namespace Howest.MagicCards.Web.Data;

public class DeckService : IDeckService
{
	private readonly HttpClient _client;
	private readonly JsonSerializerOptions _jsonOptions;

	public DeckService(IHttpClientFactory httpClientFactory)
	{
		_client = httpClientFactory.CreateClient("DeckAPI");
		_jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};
	}

	public async Task<IEnumerable<MinimalAPI.Models.Card>> GetDeckAsync()
	{
		HttpResponseMessage response = await _client.GetAsync("cards");

		if (response.IsSuccessStatusCode)
		{
			string content = await response.Content.ReadAsStringAsync();
			IEnumerable<MinimalAPI.Models.Card> cards = JsonSerializer.Deserialize<IEnumerable<MinimalAPI.Models.Card>>(content, _jsonOptions);
			return cards;
		}
		else
		{
			throw new Exception("Something went wrong");
		}
	}

	public async Task RemoveCard(MinimalAPI.Models.Card card)
	{
		HttpResponseMessage response = await _client.DeleteAsync($"cards?id={card.Id}");
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("Something went wrong");
		}
	}

	public void AddCard(MinimalAPI.Models.Card card)
	{

	}

	public Task UpdateCard(MinimalAPI.Models.Card card)
	{
		throw new NotImplementedException();
	}
}
