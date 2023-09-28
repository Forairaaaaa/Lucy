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
using static System.Net.Mime.MediaTypeNames;

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

            // Init color map
            _ansiColorMap = GetAnsiColorMap();

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

            // Handle clear 
            if (ViewModel.ClearFlag)
            {
                ViewModel.ClearFlag = false;
                TextBlockReceivedMessage.Inlines.Clear();
            }

            // Handle received message 
            if (ViewModel.SerialPortService.Available() > 0)
            {
                //// Update received message with out ansi decode 
                //ReceivePanelPopMessage(ViewModel.SerialPortService.Read());



                // Update received message with ANSI decode 
                _ansiResultList = ViewModel.SerialPortService.ReadWithAnsiDecode();
                foreach (var ansiResult in _ansiResultList)
                {
                    //ReceivePanelPopMessage(ansiResult.Message);
                    ReceivePanelPopMessage(ansiResult.Message, ansiResult.Value);
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

        /// <summary>
        /// Pop message by adding Run under Inlines 
        /// </summary>
        /// <param name="message"></param>
        private void ReceivePanelPopMessage(string message, string? ansiValue = null)
        {
            // Create an run 
            var run = new Run();

            run.Text = message;

            // Get ANSI color 
            var colorBrush = GetAnsiColorBrush(ansiValue);
            if (colorBrush != null)
            {
                run.Foreground = colorBrush;
            }

            // Add into text block 
            TextBlockReceivedMessage.Inlines.Add(run);
        }

        private readonly Dictionary<string, SolidColorBrush> _ansiColorMap;

        private SolidColorBrush? GetAnsiColorBrush(string? ansiValue)
        {
            if (ansiValue == null)
            {
                return null;
            }

            // Check exist 
            SolidColorBrush? result;
            if (_ansiColorMap.TryGetValue(ansiValue, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Bind ANSI value to color brush 
        /// https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797#color-codes
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, SolidColorBrush> GetAnsiColorMap()
        {
            var map = new Dictionary<string, SolidColorBrush>();

            // TODO 
            // This shit looks suck
            // Miss material already :(
            //map.Add("[0;30m", new SolidColorBrush(Microsoft.UI.Colors.White));
            //map.Add("[0;31m", new SolidColorBrush(Microsoft.UI.Colors.Red));
            //map.Add("[0;32m", new SolidColorBrush(Microsoft.UI.Colors.Green));
            //map.Add("[0;33m", new SolidColorBrush(Microsoft.UI.Colors.Yellow));
            //map.Add("[0;34m", new SolidColorBrush(Microsoft.UI.Colors.Blue));
            //map.Add("[0;35m", new SolidColorBrush(Microsoft.UI.Colors.Magenta));
            //map.Add("[0;36m", new SolidColorBrush(Microsoft.UI.Colors.Cyan));
            //map.Add("[0;37m", new SolidColorBrush(Microsoft.UI.Colors.White));
            //map.Add("[0m", new SolidColorBrush(Microsoft.UI.Colors.White));

            return map;
        }
    }
}
