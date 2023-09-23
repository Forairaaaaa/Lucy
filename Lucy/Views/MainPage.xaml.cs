using System.Drawing;
using Lucy.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Lucy.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
           
        // Set title bar 
        App.MainWindow.SetTitleBar(MainPageTitleBar);
    }
}
