using System.Drawing;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Lucy.Services;
using Lucy.ViewModels;

using Microsoft.UI.Xaml.Controls;

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
