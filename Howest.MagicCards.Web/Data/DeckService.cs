using System.Text;
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
		HttpResponseMessage existingCard = await _client.GetAsync($"card?name={card.Name}");
		if (existingCard.IsSuccessStatusCode)
		{
			string content = await existingCard.Content.ReadAsStringAsync();
			MinimalAPI.Models.Card existingCardObject = JsonSerializer.Deserialize<MinimalAPI.Models.Card>(content, _jsonOptions);
			if (existingCardObject.Amount > 1)
			{
				existingCardObject.Amount--;
				string json = JsonSerializer.Serialize(existingCardObject);
				StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _client.PutAsync("card", data);
				if (response.IsSuccessStatusCode)
				{
					// FIGURE OUT RESPONSE TO KNOW THAT THERE IS A UPDATED CARD
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
					// FIGURE OUT RESPONSE TO KNOW THAT THERE IS A DELETED CARD
				}
				else
				{
					throw new Exception("Could not delete card");
				}
			}

		}
	}

	public void AddCard(MinimalAPI.Models.Card card)
	{
		// TODO: Add card to deck
		// TODO: Check if card already exists in deck
	}

	public Task UpdateCard(MinimalAPI.Models.Card card)
	{
		throw new NotImplementedException();
	}
}
