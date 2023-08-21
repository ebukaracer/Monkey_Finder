using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Monkey_Finder.Models;

namespace Monkey_Finder.ViewModels;

// [QueryProperty(nameof(Monkey), "monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IMap _map;

    public MonkeyDetailsViewModel(IMap map)
    {
        _map = map;
    }

    [ObservableProperty]
    private Monkey _monkey;

    partial void OnMonkeyChanged(Monkey value)
    {
        Title = value.Name;
    }

    [RelayCommand]
    private async Task OpenMap()
    {
        try
        {
            await _map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
            {
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, No Maps app available :(", ex.Message, "Ok");
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Monkey = query["monkey"] as Monkey ?? throw new InvalidOperationException("Monkey was null");
    }
}