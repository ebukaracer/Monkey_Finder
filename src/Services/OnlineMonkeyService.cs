using System.Net.Http.Json;
using Monkey_Finder.Models;

namespace Monkey_Finder.Services;

public class OnlineMonkeyService : IMonkeyService
{
    private HttpClient _httpClient;

    private List<Monkey> _monkeyList = new();


    public OnlineMonkeyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Monkey>> GetMonkeys()
    {
        if (_monkeyList.Count > 0)
            return _monkeyList;

        var response = await _httpClient.GetAsync("https://www.montemagno.com/monkeys.json");

        if (response.IsSuccessStatusCode)
            _monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();

        return _monkeyList;
    }
}