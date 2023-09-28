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
        private List<AnsiResult> _ansiResultList = new();

        private void UpdateReceivedMessage(object? sender, object e)
        {
            if (_isNeedToScroll)
            {
                ScrollViewerReceivedMessage.ChangeView(null, ScrollViewerReceivedMessage.ScrollableHeight, null);
                // Reset flag 
                _isNeedToScroll = false;
            }

            // Handle received message 
            if (ViewModel.SerialPortService.Available() > 0)
            {
                //// Update received message with out ansi decode 
                //ViewModel.ReceivedMessageBuffer += ViewModel.SerialPortService.Read();



                // Update received message with ANSI decode 
                _ansiResultList = ViewModel.SerialPortService.ReadWithAnsiDecode();
                foreach (var ansiResult in _ansiResultList)
                {
                    ReceivePanelPopMessage(ansiResult.Message);
                }


                // Update status label 
                ViewModel.UpdateIoStatusLabel();

                // Scroll to bottom 
                ScrollViewerReceivedMessage.ChangeView(null, ScrollViewerReceivedMessage.ScrollableHeight, null);

                // Set flag, to notice one more scroll after done reading 
                // Invoke 'ChangeView' right after every update will not reach the actual bottom 
                // Need a render to update some scrollviewer's properties I guess 
                _isNeedToScroll = true;
            }

            // Handle error 
            if (ViewModel.ErrorBuffer.Length != 0)
            {
                // Pop error 
                //ViewModel.ReceivedMessageBuffer += ViewModel.ErrorBuffer;
                ReceivePanelPopMessage(ViewModel.ErrorBuffer);

                // Empty buffer 
                ViewModel.ErrorBuffer = string.Empty;

                _isNeedToScroll = true;
            }

            // Check connection
            ViewModel.CheckConection();
        }

        private void ReceivePanelPopMessage(string message)
        {
            // Create an run 
            var run = new Run
            {
                // TODO 
                // Change color or some shit 
                Text = message
            };

            // Add into text block 
            TextBlockReceivedMessage.Inlines.Add(run);
        }
    }
}
