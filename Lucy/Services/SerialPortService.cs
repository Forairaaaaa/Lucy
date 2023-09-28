using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucy.Contracts.Services;
using Lucy.Helpers;
using WinUIEx.Messaging;

namespace Lucy.Services;
public class SerialPortService : ISerialPortService
{
    public bool IsOpened => _serialPort.IsOpen;

    public string PortName
    {
        get => _serialPort.PortName;
        set => _serialPort.PortName = value;
    }

    public string BaudRate
    {
        get => _serialPort.BaudRate.ToString();
        set
        {
            try
            {
                _serialPort.BaudRate = Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public string LastError
    {
        get;
        set;
    }

    public int SendedMessageNum => _sendedMessageNum;

    public int ReceivedMessageNum => _receivedMessageNum;

    private int _sendedMessageNum;

    private int _receivedMessageNum;

    // Serial port 
    private readonly SerialPort _serialPort;

    /// <summary>
    /// Constructor 
    /// </summary>
    public SerialPortService()
    {
        // Create instance 
        _serialPort = new SerialPort
        {
            ReadTimeout = 1000,
            WriteTimeout = 1000
        };

        // Default value 
        PortName = "COM1";
        BaudRate = "115200";
        LastError = string.Empty;
        _sendedMessageNum = 0;
        _receivedMessageNum = 0;
    }

    /// <summary>
    /// Open port 
    /// </summary>
    /// <returns></returns>
    public bool Open()
    {
        if (IsOpened)
        {
            return true;
        }
        
        try 
        {
            // Try open port 
            _serialPort.Open();
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            LastError = ex.Message;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Close port 
    /// </summary>
    /// <returns></returns>
    public bool Close()
    {
        if (!IsOpened)
        {
            return true;
        }
 
        try
        {
            // Try close 
            _serialPort.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            LastError = ex.Message;
            return false;
        }

        return true;
    }

    public void ClearCountNum()
    {
        _sendedMessageNum = 0;
        _receivedMessageNum = 0;
    }

    /// <summary>
    /// Write message
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public bool Write(string message)
    {
        //Console.WriteLine(message.Length);
        //Console.WriteLine(message);

        //Console.WriteLine(_serialPort.PortName);
        //Console.WriteLine(_serialPort.BaudRate);
        //Console.WriteLine(message);

        if (!IsOpened)
        {
            LastError = "Error_PortNotOpen".GetLocalized();
            return false;
        }

        try
        {
            _serialPort.Write(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            LastError = ex.Message;
            return false;
        }

        // Update counting num 
        _sendedMessageNum += message.Length;

        return true;
    }
        
    /// <summary>
    /// Return how many bytes can be read 
    /// </summary>
    /// <returns></returns>
    public int Available()
    {
        if (_serialPort.IsOpen)
        {
            return _serialPort.BytesToRead;
        }

        return 0;
    }

    /// <summary>
    /// Read existing data as string
    /// </summary>
    /// <returns></returns>
    public string Read()
    {
        if (_serialPort.IsOpen)
        {
            try
            {
                if (_serialPort.BytesToRead > 0)
                {
                    var result = _serialPort.ReadExisting();

                    // Update counting num 
                    _receivedMessageNum += result.Length;

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LastError = ex.Message;
            }
        }

        return "";
    }

    public string[] BaudRateList()
    {
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

        return baudRatesList;
    }
}
