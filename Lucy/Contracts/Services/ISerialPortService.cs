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

    string LastError
    {
        get; set;
    }

    int SendedMessageNum
    {
        get;
    }

    int ReceivedMessageNum
    {
        get;
    }

    string[] BaudRateList();

    bool Open();

    bool Close();

    bool Write(string message);

    int Available();

    string Read();

    void ClearCountNum();
}