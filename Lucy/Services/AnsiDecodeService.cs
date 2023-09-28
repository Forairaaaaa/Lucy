using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;

namespace Lucy.Services;
internal class AnsiDecodeService
{
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

    private readonly Dictionary<string, SolidColorBrush> _ansiColorMap;

    public AnsiDecodeService()
    {
        // Get color brush map
        _ansiColorMap = GetAnsiColorMapByTheme();
    }

    /// <summary>
    /// Bind ANSI value to color brush 
    /// https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797#color-codes
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, SolidColorBrush> GetAnsiColorMapByTheme()
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

    /// <summary>
    /// Only support color now 
    /// https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797
    /// https://gist.github.com/JBlond/2fea43a3049b38287e5e9cefc87b2124
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public List<AnsiResult> AnsiEscapeDecode(string message)
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
    /// Get color brush that ANSI matched from map 
    /// </summary>
    /// <param name="ansiValue"></param>
    /// <returns></returns>
    public SolidColorBrush? GetAnsiColorBrush(string? ansiValue)
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
}
