using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Monkey_Finder.Models;
using Monkey_Finder.Services;
using Monkey_Finder.Views;

namespace Monkey_Finder.ViewModels;

public partial class MonkeyViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool _isRefreshing;

    private readonly IMonkeyService _monkeyService;
    private readonly IConnectivity _connectivity;
    private readonly IGeolocation _geolocation;

    public ObservableCollection<Monkey> Monkeys { get; } = new();


    public MonkeyViewModel(IMonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";

        _monkeyService = monkeyService;
        _connectivity = connectivity;
        _geolocation = geolocation;
    }

    [RelayCommand]
    public async Task GetMonkeyAsync()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            ClearMonkeys();
            IsRefreshing = false;

            await Shell.Current.DisplayAlert("No connectivity :(",
                "Please check your internet and try again.", "Ok");

            return;
        }

        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

            var monkeys = await _monkeyService.GetMonkeys();

            ClearMonkeys();

            foreach (var monkey in monkeys)
                Monkeys.Add(monkey);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    private void ClearMonkeys()
    {
        if (Monkeys.Count != 0)
            Monkeys.Clear();
    }

    [RelayCommand]
    private async Task GoToDetails(Monkey monkey)
    {
        if (monkey == null || IsBusy || IsRefreshing)
            return;

        var parameter = new Dictionary<string, object> { { nameof(monkey), monkey } };
        await Shell.Current.GoToAsync(nameof(DetailsPage), true, parameter);
    }

    [RelayCommand]
    private async Task GetClosestMonkey(Monkey monkey)
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            // Get cached location, else get real location.
            var location = await _geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await _geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            // Find closest monkey to us
            var first = Monkeys.MinBy(m => location.CalculateDistance(
                new Location(m.Latitude, m.Longitude), DistanceUnits.Miles));

            await Shell.Current.DisplayAlert("", first?.Name + " " +
                                                 first?.Location, "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}