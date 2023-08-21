using Monkey_Finder.Services;
using Monkey_Finder.ViewModels;

namespace Monkey_Finder
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MonkeyViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}