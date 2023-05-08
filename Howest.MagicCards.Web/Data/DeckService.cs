using System.Text;
using System.Text.Json;
using Card = Howest.MagicCards.MinimalAPI.Models.Card;

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

	public async Task<IEnumerable<Card>> GetDeckAsync()
	{
		HttpResponseMessage response = await _client.GetAsync("cards");

		if (response.IsSuccessStatusCode)
		{
			string content = await response.Content.ReadAsStringAsync();
			IEnumerable<Card> cards = JsonSerializer.Deserialize<IEnumerable<Card>>(content, _jsonOptions);
			return cards;
		}
		else
		{
			throw new Exception("Something went wrong");
		}
	}

	public async Task<bool> RemoveCard(Card card)
	{
		HttpResponseMessage existingCard = await _client.GetAsync($"card?name={card.Name}");
		if (existingCard.IsSuccessStatusCode)
		{
			string content = await existingCard.Content.ReadAsStringAsync();
			Card existingCardObject = JsonSerializer.Deserialize<Card>(content, _jsonOptions);
			if (existingCardObject.Amount > 1)
			{
				existingCardObject.Amount--;
				string json = JsonSerializer.Serialize(existingCardObject);
				StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _client.PutAsync("cards", data);
				if (response.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					throw new Exception("Could not update card");
				}
			}
			else
			{
				HttpResponseMessage response = await _client.DeleteAsync($"card/{card.Id}");
				if (response.IsSuccessStatusCode)
				{
					return true;
				}
				else
				{
					throw new Exception("Could not delete card");
				}
			}

		}
		else
		{
			throw new Exception("Card does not exist");
		}
	}

	public void AddCard(Card card)
	{
		// TODO: Add card to deck
		// TODO: Check if card already exists in deck
	}

	public Task UpdateCard(MinimalAPI.Models.Card card)
	{
		throw new NotImplementedException();
	}
}
