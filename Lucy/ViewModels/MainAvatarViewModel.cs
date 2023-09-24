using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lucy.Contracts.Services;
using Microsoft.UI.Xaml.Navigation;

namespace Lucy.ViewModels
{
    public partial class MainAvatarViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private bool isBackEnabled;

        public ICommand GoPageSettingsCommand
        {
            get;
        }

        public INavigationService NavigationService
        {
            get;
        }

        public MainAvatarViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;

            GoPageSettingsCommand = new RelayCommand(OnGoPageSettings);
        }

        private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

        private void OnGoPageSettings() => NavigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
    }
}
