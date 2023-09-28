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
using Lucy.Helpers;
using Lucy.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

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

        [ObservableProperty]
        private string ioStatusLabel;

        [ObservableProperty]
        private string openPortButtonContent;

        [ObservableProperty]
        private string openPortButtonToolTip;

        public string ErrorBuffer;

        public bool ClearFlag;

        // Compare to actaul port state for connection checking 
        private bool _isOpenedAssume;

        public ICommand UpdateAvailablePorts
        {
            get;
        }

        public ICommand SendMessage
        {
            get;
        }

        public ICommand ClearAll
        {
            get;
        }

        public ICommand OpenPort
        {
            get;
        }

        // Serial port service 
        private readonly ISerialPortService _serialPortService;

        // Expose for code behind usage 
        public ISerialPortService SerialPortService => _serialPortService;

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
            ClearAll = new RelayCommand(OnClearAll);
            OpenPort = new RelayCommand(OnOpenPort);

            // Default value 
            selectedPortName = _serialPortService.PortName;
            selectedBaudRate = _serialPortService.BaudRate;
            availableBaudRateFlyout = GetBaudRateMenuFlyout();
            sendMessageBuffer = string.Empty;
            ioStatusLabel = string.Empty;
            openPortButtonContent = GetOpenPortButtonContent();
            openPortButtonToolTip = GetOpenPortButtonToolTip();
            ErrorBuffer = string.Empty;
            _isOpenedAssume = _serialPortService.IsOpened;
            ClearFlag = false;

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
                // Try close 
                if (!_serialPortService.Close())
                {
                    UpdateOpenPortButton();
                    return;
                }

                // Update selected port 
                SelectedPortName = selectedPortName;

                // Update service 
                _serialPortService.PortName = SelectedPortName;

                // Open again
                _serialPortService.Open();

                UpdateOpenPortButton();
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

            // Try write message 
            if (!_serialPortService.Write(SendMessageBuffer))
            {
                PopError();
                return;
            }

            // Update status label 
            UpdateIoStatusLabel();
        }

        public void UpdateIoStatusLabel()
        {
            IoStatusLabel = _serialPortService.ReceivedMessageNum.ToString() + " - " + _serialPortService.SendedMessageNum.ToString();
        }

        private void OnClearAll()
        {
            _serialPortService.ClearCountNum();

            // Set flag to notive reveice timer
            ClearFlag = true;

            // Update status label
            UpdateIoStatusLabel();
        }

        private void OnOpenPort()
        {
            if (_serialPortService.IsOpened)
            {
                // Try close
                if (!_serialPortService.Close())
                {
                    PopError();
                }
            }
            else
            {
                // Try open 
                _serialPortService.Open();
                if (!_serialPortService.Open())
                {
                    PopError();
                }
            }

            UpdateOpenPortButton();
        }

        private string GetOpenPortButtonContent()
        {
            return _serialPortService.IsOpened ? "OpenPortButton_IsOpened".GetLocalized() : "OpenPortButton_IsClosed".GetLocalized();
        }

        private string GetOpenPortButtonToolTip()
        {
            return _serialPortService.IsOpened ? "ToolTip_PortIsOpened".GetLocalized() : "ToolTip_PortIsClosed".GetLocalized();
        }

        private void UpdateOpenPortButton()
        {
            OpenPortButtonContent = GetOpenPortButtonContent();
            OpenPortButtonToolTip = GetOpenPortButtonToolTip();
            _isOpenedAssume = _serialPortService.IsOpened;
        }

        private void PopError(string? custom = null)
        {
            // Recevie panel's timer tick will handle this 
            if (custom == null)
            {
                ErrorBuffer += "Error_Head".GetLocalized() + _serialPortService.LastError + "\r\n";
            }
            else
            {
                ErrorBuffer += "Error_Head".GetLocalized() + custom + "\r\n";
            }
        }

        public void CheckConection()
        {
            if (_serialPortService.IsOpened != _isOpenedAssume)
            {
                // Update state 
                UpdateOpenPortButton();

                // Pop error 
                PopError("Error_ConnectionLost".GetLocalized());
            }
        }
    }
}
