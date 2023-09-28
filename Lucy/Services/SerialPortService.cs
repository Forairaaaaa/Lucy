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

    /// <summary>
    /// Only support color now 
    /// https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797
    /// https://gist.github.com/JBlond/2fea43a3049b38287e5e9cefc87b2124
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private List<AnsiResult> AnsiEscapeDecode(string message)
    {
        var result = new List<AnsiResult>();

        // To simplfy the mixed use situation 
        message = "\u001b[0m" + message;

        // Split messge by Escape (UTF-8)
        var splitedList = message.Split('\u001b', StringSplitOptions.RemoveEmptyEntries);

        // Get color value and add into list 
        foreach (var chunk in splitedList)
        {
            // If it's empty reset ending 
            if (chunk.Equals("[0m"))
                continue;

            // Get value 
            var messageStartIndex = chunk.IndexOf('m') + 1;

            // Add result 
            result.Add(new AnsiResult(chunk[..messageStartIndex], chunk[messageStartIndex..]));
        }

        return result;
    }

    /// <summary>
    /// Simple wrap 
    /// </summary>
    /// <returns></returns>
    public List<AnsiResult> ReadWithAnsiDecode()
    {
        return AnsiEscapeDecode(Read());
    }
}
