using System.Text.Json;
using Monkey_Finder.Models;

namespace Monkey_Finder.Services;

public class OfflineMonkeyService : IMonkeyService
{
    private List<Monkey> _monkeyList = new();

    public async Task<List<Monkey>> GetMonkeys()
    {
        await using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        _monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);

        return _monkeyList;
    }
}