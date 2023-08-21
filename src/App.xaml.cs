// ReSharper disable PossibleNullReferenceException

using System.Diagnostics;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Monkey_Finder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }


#if WINDOWS
        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);
            window.Activated += Window_Activated;
            return window;
        }

        private static async void Window_Activated(object sender, EventArgs e)
        {
            const int defaultWidth = 400;
            const int defaultHeight = 720;

            var window = sender as Window;

            // change window size.
            window.Width = defaultWidth;
            window.Height = defaultHeight;

            // give it some time to complete window resizing task.
            await window.Dispatcher.DispatchAsync(() => { });

            var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

            // move to screen left
            window.X = (displayInfo.Width / displayInfo.Density - window.Width);
            window.Y = (displayInfo.Height / displayInfo.Density - window.Height) / 20;
        }
#endif
    }
}