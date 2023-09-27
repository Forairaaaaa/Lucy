using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Lucy.ViewModels;
using Microsoft.UI.Xaml.Documents;
using Lucy.Contracts.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Lucy.Views
{
    public sealed partial class MainPageReceivePanel : UserControl
    {
        public MainControlPanelViewModel ViewModel
        {
            get;
        }

        private readonly DispatcherTimer _updateReceivedMessageTimer;

        public MainPageReceivePanel()
        {
            // Use control panel's view model too 
            ViewModel = App.GetService<MainControlPanelViewModel>();
            this.InitializeComponent();

            // Setup timer 
            _updateReceivedMessageTimer = new DispatcherTimer();
            _updateReceivedMessageTimer.Tick += UpdateReceivedMessage;
            _updateReceivedMessageTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _updateReceivedMessageTimer.Start();
        }

        private bool _isNeedToScroll = false;

        private void UpdateReceivedMessage(object? sender, object e)
        {
            if (_isNeedToScroll)
            {
                ScrollViewerReceivedMessage.ChangeView(null, ScrollViewerReceivedMessage.ScrollableHeight, null);
                // Reset flag 
                _isNeedToScroll = false;
            }

            if (ViewModel.SerialPortService.Available() > 0)
            {
                // Update received message 
                ViewModel.ReceivedMessageBuffer += ViewModel.SerialPortService.Read();
                ViewModel.UpdateIoStatusLabel();

                // Scroll to bottom 
                ScrollViewerReceivedMessage.ChangeView(null, ScrollViewerReceivedMessage.ScrollableHeight, null);

                // Set flag, to notice one more scroll after done reading 
                // Invoke 'ChangeView' right after every update will not reach the actual bottom 
                // Need a render to update some scrollviewer's properties I guess 
                _isNeedToScroll = true;
            }

            if (ViewModel.ErrorBuffer.Length != 0)
            {
                // Pop error 
                ViewModel.ReceivedMessageBuffer += ViewModel.ErrorBuffer;

                // Empty buffer 
                ViewModel.ErrorBuffer = string.Empty;

                _isNeedToScroll = true;
            }
        }
    }
}
