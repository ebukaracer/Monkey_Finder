using Monkey_Finder.Models;

namespace Monkey_Finder.Services;

public interface IMonkeyService
{
    Task<List<Monkey>> GetMonkeys();
}