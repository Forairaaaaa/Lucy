using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Contracts.Services;
public interface ISerialPortService
{
    bool IsOpened
    {
        get;
    }

    string PortName
    {
        get; set; 
    }

    string BaudRate
    {
        get; set; 
    }

    bool Open();

    bool Close();

    bool Write(string message);

    event EventHandler<string>? MessageReceived;
}
