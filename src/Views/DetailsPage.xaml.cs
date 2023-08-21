using Monkey_Finder.ViewModels;

namespace Monkey_Finder.Views;

public partial class DetailsPage : ContentPage
{
    public DetailsPage(MonkeyDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}