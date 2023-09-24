using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucy.Contracts.Services;

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
            }
        }
    }

    private readonly SerialPort _serialPort;

    /// <summary>
    /// Constructor 
    /// </summary>
    public SerialPortService()
    {
        // Create instance 
        _serialPort = new SerialPort();

        // Default value 
        PortName = "COM1";
        BaudRate = "115200";
    }

    /// <summary>
    /// Open port 
    /// </summary>
    /// <returns></returns>
    public bool Open()
    {
        if (IsOpened)
        {
            return false;
        }
        
        // Try open 
        try 
        {
            _serialPort.Open();
        } 
        catch (Exception ex)
        {
        }

        return IsOpened;
    }

    /// <summary>
    /// Close port 
    /// </summary>
    /// <returns></returns>
    public bool Close()
    {
        if (!IsOpened)
        {
            return false;
        }

        // Try close 
        try
        {
            _serialPort.Close();
        }
        catch (Exception ex)
        {
        }

        return IsOpened;
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

        Console.WriteLine(_serialPort.PortName);
        Console.WriteLine(_serialPort.BaudRate);
        Console.WriteLine(message);

        if (!IsOpened)
        {
            try
            {
                _serialPort.Write(message);
            }
            catch(Exception ex) 
            {
                return false;
            }

            return true;
        }

        return true;
    }

    /// <summary>
    /// Read message 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public string ReadLine(string message)
    {
        return "???";
    }
}
