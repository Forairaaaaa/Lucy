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
                Console.WriteLine(ex.Message);
            }
        }
    }

    // Serial port 
    private readonly SerialPort _serialPort;

    //// Reading flag 
    //private bool _isReading = false;
    //// Reading thread 
    //private Thread? _readingThread;

    //// Message received event handler 
    //public event EventHandler<string>? MessageReceived;

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

            //// Start reading thread 
            //_isReading = true;
            //_readingThread = new Thread(ReadingSerial);
            //_readingThread.Start();
        } 
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
            //// Stop reading thread and wait 
            //_isReading = false;
            //_readingThread?.Join();

            // Try close 
            _serialPort.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        return true;
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
            return false;
        }

        try
        {
            _serialPort.Write(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    //private void ReadingSerial()
    //{
    //    while (_isReading)
    //    {
    //        try
    //        {
    //            if (_serialPort.BytesToRead > 0)
    //            {
    //                var message = _serialPort.ReadExisting();
    //                //Console.WriteLine(message);

    //                // Fire event
    //                MessageReceived?.Invoke(this, message);
    //            }
    //        }
    //        catch(Exception ex)
    //        {
    //            Console.WriteLine(ex.Message);
    //            break;
    //        }
    //    }
    //}

    public int Available()
    {
        if (_serialPort.IsOpen)
        {
            return _serialPort.BytesToRead;
        }

        return 0;
    }

    public string Read()
    {
        if (_serialPort.IsOpen)
        {
            try
            {
                if (_serialPort.BytesToRead > 0)
                {
                    return _serialPort.ReadExisting();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        return "";
    }
}
