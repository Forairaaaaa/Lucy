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
        public MainReceivePanelViewModel ViewModel
        {
            get;
        }

        DispatcherTimer _updateReceivedMessageTimer;

        public MainPageReceivePanel()
        {
            ViewModel = App.GetService<MainReceivePanelViewModel>();
            this.InitializeComponent();


            // Setup timer 
            _updateReceivedMessageTimer = new DispatcherTimer();
            _updateReceivedMessageTimer.Tick += UpdateReceivedMessage;
            _updateReceivedMessageTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _updateReceivedMessageTimer.Start();
        }

        private void UpdateReceivedMessage(object? sender, object e)
        {
            if (ViewModel.SerialPortService.Available() > 0)
            {
                TextBlockReceivedMessage.Text += ViewModel.SerialPortService.Read();
                ScrollViewerReceivedMessage.ScrollToVerticalOffset(ScrollViewerReceivedMessage.ScrollableHeight);
            }
        }
    }
}
