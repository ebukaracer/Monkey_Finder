using Microsoft.Extensions.Logging;
using Monkey_Finder.Services;
using Monkey_Finder.ViewModels;
using Monkey_Finder.Views;

namespace Monkey_Finder
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DetailsPage>();
            builder.Services.AddSingleton<IMonkeyService>(new OfflineMonkeyService());
            builder.Services.AddSingleton<MonkeyViewModel>();
            builder.Services.AddTransient<MonkeyDetailsViewModel>();
            builder.Services.AddSingleton(Connectivity.Current);
            builder.Services.AddSingleton(Geolocation.Default);
            builder.Services.AddSingleton(Map.Default);
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}