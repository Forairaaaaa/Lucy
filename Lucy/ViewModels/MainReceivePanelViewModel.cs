using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Lucy.Contracts.Services;
using Lucy.Services;
using Lucy.Views;
using Microsoft.UI.Xaml.Documents;

namespace Lucy.ViewModels
{
    public partial class MainReceivePanelViewModel : ObservableRecipient
    {
        // Serial port service 
        public ISerialPortService SerialPortService;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="serialPortService"></param>
        public MainReceivePanelViewModel(ISerialPortService serialPortService)
        {
            // Haven't find an elegant way to control srollviewer in view modle 
            // So just expose the serial service the code behind :(
            SerialPortService = serialPortService;
        }
    }
}
