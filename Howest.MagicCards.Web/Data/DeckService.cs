﻿using System.Text;
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
        string name = card.Name.Replace(" ", "%20");
        HttpResponseMessage existingCardResponse = await _client.GetAsync($"card?name={name}");

        if (!existingCardResponse.IsSuccessStatusCode)
			throw new Exception("Card does not exist");

		Card existingCardObject = await GetCardFromResponse(existingCardResponse);

		if (existingCardObject.Amount > 1)
			return await UpdateCardAmount(existingCardObject, -1);
		else
			return await DeleteCard(card.Id);
	}

	public async Task<bool> AddCard(Card card)
	{
		string name = card.Name.Replace(" ", "%20");
		HttpResponseMessage response = await _client.GetAsync($"card?name={name}");

		if (!response.IsSuccessStatusCode)
		{
			string json = JsonSerializer.Serialize(card);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage postResponse = await _client.PostAsync("cards", data);
			if (!postResponse.IsSuccessStatusCode)
				throw new Exception("Could not add card");
			else
				return true;
		} else
		{
			Card returnedCard = await GetCardFromResponse(response);
			return await UpdateCardAmount(returnedCard, 1);
		}
	}

	#region Help functions

	private async Task<Card> GetCardFromResponse(HttpResponseMessage response)
	{
		string content = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<Card>(content, _jsonOptions);
	}

	private async Task<bool> UpdateCardAmount(Card card, int amountChange)
	{
		card.Amount += amountChange;
		string json = JsonSerializer.Serialize(card);
		StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

		HttpResponseMessage updateResponse = await _client.PutAsync("cards", data);
		if (!updateResponse.IsSuccessStatusCode)
			throw new Exception("Could not update card");

		return true;
	}


	private async Task<bool> DeleteCard(string cardId)
	{
		HttpResponseMessage deleteResponse = await _client.DeleteAsync($"card/{cardId}");
		if (!deleteResponse.IsSuccessStatusCode)
			throw new Exception("Could not delete card");

		return true;
	}
	#endregion
}
