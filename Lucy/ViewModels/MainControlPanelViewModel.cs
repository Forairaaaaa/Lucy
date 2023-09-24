using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lucy.Contracts.Services;
using Lucy.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Lucy.ViewModels
{
    public partial class MainControlPanelViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private string selectedPortName;

        [ObservableProperty]
        private MenuFlyout availablePortsFlyout;

        [ObservableProperty]
        private string selectedBaudRate;

        [ObservableProperty]
        private MenuFlyout availableBaudRateFlyout;

        [ObservableProperty]
        private string sendMessageBuffer;

        public ICommand UpdateAvailablePorts
        {
            get;
        }

        public ICommand SendMessage
        {
            get;
        }

        // Serial port service 
        private readonly ISerialPortService _serialPortService;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="serialPortService"></param>
        public MainControlPanelViewModel(ISerialPortService serialPortService)
        {
            _serialPortService = serialPortService;

            // Commands
            UpdateAvailablePorts = new RelayCommand(OnUpdateAvailablePorts);
            SendMessage = new RelayCommand(OnSendMessage);

            // Default value 
            selectedPortName = _serialPortService.PortName;
            selectedBaudRate = _serialPortService.BaudRate;
            availableBaudRateFlyout = GetBaudRateMenuFlyout();
            sendMessageBuffer = "";

            // Available ports flyout
            availablePortsFlyout = new MenuFlyout();
            availablePortsFlyout.Placement = Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Bottom;
        }

        /// <summary>
        /// Get available serial ports and update menu flyou
        /// </summary>
        private void OnUpdateAvailablePorts()
        {
            // Clear 
            AvailablePortsFlyout.Items.Clear();

            // Get available ports
            var availableSerialPorts = SerialPort.GetPortNames();

            foreach (var portName in availableSerialPorts)
            {
                // New item
                var newFlyoutItem = new MenuFlyoutItem();
                // Set text and handle click event
                newFlyoutItem.Text = portName;
                newFlyoutItem.Click += (object sender, RoutedEventArgs e) => 
                {
                    // Call handler 
                    OnUpdateSelectedPort(newFlyoutItem.Text);
                };
                // Push into flyout
                AvailablePortsFlyout.Items.Add(newFlyoutItem);
            }
        }

        private void OnUpdateSelectedPort(string selectedPortName)
        {
            // If already opened 
            if (_serialPortService.IsOpened)
            {
                // Don't know why it's false
                //var shit2 = App.GetService<MainPageTitleBar>().SwitchOpenPort.IsOn;
                //Console.WriteLine(shit2);

                // Try close 
                if (!_serialPortService.Close())
                {
                    return;
                }

                // Update selected port 
                SelectedPortName = selectedPortName;

                // Update service 
                _serialPortService.PortName = SelectedPortName;

                // Reset switch (kinf of failed to do that, don't know why it's false already, set to false again doesn't change the ui

                // But if I set it true here
                // This will trigger the toggle event and open the port
                // It kind of achieve the hot port changing... 
                App.GetService<MainPageTitleBar>().SwitchOpenPort.IsOn = true;
            }
            else
            {
                // Update selected port 
                SelectedPortName = selectedPortName;

                // Update service 
                _serialPortService.PortName = SelectedPortName;
            }
        }

        private MenuFlyout GetBaudRateMenuFlyout()
        {
            var availableBaudRateFlyout = new MenuFlyout();
            availableBaudRateFlyout.Placement = Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Bottom;

            string[] baudRatesList = {
                "300",
                "600",
                "1200",
                "2400",
                "4800",
                "9600",
                "14400",
                "19200",
                "38400",
                "56000",
                "57600",
                "115200",
                "128000",
                "256000",
                "460800",
                "512000",
                "750000",
                "921600",
                "1500000",
            };

            foreach (var baudRate in baudRatesList)
            {
                // New item
                var newFlyoutItem = new MenuFlyoutItem();
                // Set text and handle click event
                newFlyoutItem.Text = baudRate;
                
                newFlyoutItem.Click += (object sender, RoutedEventArgs e) =>
                {
                    // Update selected baud rate
                    SelectedBaudRate = newFlyoutItem.Text;

                    // Update service
                    _serialPortService.BaudRate = SelectedBaudRate;
                };
                // Push into flyout
                availableBaudRateFlyout.Items.Add(newFlyoutItem);
            }

            return availableBaudRateFlyout;
        }

        private void OnSendMessage()
        {
            // Get message 
            //Console.WriteLine(SendMessageBuffer.Length);
            //Console.Write(SendMessageBuffer);

            if (SendMessageBuffer.Length == 0)
            {
                return;
            }

            // Write message 
            _serialPortService.Write(SendMessageBuffer);
        }
    }
}
