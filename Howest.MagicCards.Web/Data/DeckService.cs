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

	public async Task<IEnumerable<MongoCardDTO>> GetDeckAsync()
	{
		HttpResponseMessage response = await _client.GetAsync("cards");

		if (response.IsSuccessStatusCode)
		{
			string content = await response.Content.ReadAsStringAsync();
			IEnumerable<MongoCardDTO> cards = JsonSerializer.Deserialize<IEnumerable<MongoCardDTO>>(content, _jsonOptions);
			return cards;
		}
		else
		{
			throw new Exception("Something went wrong");
		}
	}

	public async Task<bool> RemoveCard(MongoCardDTO card)
    {
        string name = card.Name.Replace(" ", "%20");
        HttpResponseMessage existingCardResponse = await _client.GetAsync($"card?name={name}");

        if (!existingCardResponse.IsSuccessStatusCode)
			throw new Exception("Card does not exist");

		MongoCardDTO existingCardObject = await GetCardFromResponse(existingCardResponse);

		if (existingCardObject.Amount > 1)
			return await UpdateCardAmount(existingCardObject, -1);
		else
			return await DeleteCard(card.Id);
	}

	public async Task<bool> AddCard(MongoCardDTO card)
	{
		HttpResponseMessage generalResponse = await _client.GetAsync($"cards");
		if (generalResponse.IsSuccessStatusCode)
		{
			string content = await generalResponse.Content.ReadAsStringAsync();
			IEnumerable<MongoCardDTO> cards = JsonSerializer.Deserialize<IEnumerable<MongoCardDTO>>(content, _jsonOptions);
			int result = 0;
			foreach (MongoCardDTO cardAmount in cards)
			{
				result += cardAmount.Amount;
			}
			if (result > 61)
			{
				throw new Exception("Deck is full");
			}
		}
			string name = card.Name.Replace(" ", "%20");
		HttpResponseMessage response = await _client.GetAsync($"card?name={name}");

		if (await GetCardFromResponse(response) == null)
		{
			string json = JsonSerializer.Serialize(card);
			StringContent data = new(json, Encoding.UTF8, "application/json");

			HttpResponseMessage postResponse = await _client.PostAsync("cards", data);
			if (!postResponse.IsSuccessStatusCode)
				throw new Exception("Could not add card");
			else
				return true;
		} else
		{
			MongoCardDTO returnedCard = await GetCardFromResponse(response);
			return await UpdateCardAmount(returnedCard, 1);
		}
	}

	public async Task<bool> ClearDeck()
	{
        HttpResponseMessage response = await _client.DeleteAsync("all");
        if (!response.IsSuccessStatusCode)
            throw new Exception("Could not clear deck");
        return true;
    }

	#region Help functions

	private async Task<MongoCardDTO> GetCardFromResponse(HttpResponseMessage response)
	{
		string content = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<MongoCardDTO>(content, _jsonOptions);
	}

	private async Task<bool> UpdateCardAmount(MongoCardDTO card, int amountChange)
	{
		card.Amount += amountChange;
		string json = JsonSerializer.Serialize(card);
		StringContent data = new(json, Encoding.UTF8, "application/json");

		HttpResponseMessage updateResponse = await _client.PutAsync("cards", data);
		if (!updateResponse.IsSuccessStatusCode)
			throw new Exception("Could not update card");

		return true;
	}


	private async Task<bool> DeleteCard(string cardId)
	{
		HttpResponseMessage deleteResponse = await _client.DeleteAsync($"cards?id={cardId}");
		if (!deleteResponse.IsSuccessStatusCode)
			throw new Exception("Could not delete card");

		return true;
	}
	#endregion
}
