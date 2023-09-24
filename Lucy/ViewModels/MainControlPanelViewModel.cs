using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public ICommand UpdateAvailablePorts
        {
            get;
        }

        public MainControlPanelViewModel()
        {
            // Commands
            UpdateAvailablePorts = new RelayCommand(OnUpdateAvailablePorts);

            // Default value 
            selectedPortName = GetFirstAvailablePort();
            selectedBaudRate = "115200";
            availableBaudRateFlyout = GetBaudRateMenuFlyout();

            // Available ports flyout
            availablePortsFlyout = new MenuFlyout();
            availablePortsFlyout.Placement = Microsoft.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Bottom;
        }

        /// <summary>
        /// Get the first available port name
        /// </summary>
        /// <returns></returns>
        private static string GetFirstAvailablePort()
        {
            // Get available ports
            var availableSerialPorts = SerialPort.GetPortNames();

            // If more than COM1
            if (availableSerialPorts.Length > 1)
            {
                return availableSerialPorts[1];
            }

            return "COM1";
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
                    // Update selected port 
                    SelectedPortName = newFlyoutItem.Text;
                };
                // Push into flyout
                AvailablePortsFlyout.Items.Add(newFlyoutItem);
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
                };
                // Push into flyout
                availableBaudRateFlyout.Items.Add(newFlyoutItem);
            }

            return availableBaudRateFlyout;
        }
    }
}
