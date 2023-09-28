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

    bool Open();

    bool Close();

    bool Write(string message);

    int Available();

    string Read();

    bool CheckConnection();

    List<AnsiResult> ReadWithAnsiDecode();
}

/// <summary>
/// Decode result 
/// </summary>
public class AnsiResult
{
    public string Value
    {
        get;
    }

    public string Message
    {
        get;
    }

    public AnsiResult(string value, string message)
    {
        Value = value;
        Message = message;
    }
}
