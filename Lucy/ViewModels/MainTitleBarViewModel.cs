using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Lucy.Contracts.Services;
using Lucy.Services;

namespace Lucy.ViewModels
{
    public partial class MainTitleBarViewModel : ObservableRecipient
    {
        private readonly ISerialPortService _serialPortService;

        public MainTitleBarViewModel(ISerialPortService serialPortService)
        {
            _serialPortService = serialPortService;
        }

        public bool OnOpenSerialPort()
        {
            return _serialPortService.Open();
        }

        public bool OnCloseSerialPort()
        {
            return _serialPortService.Close();
        }
    }
}
