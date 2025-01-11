using PipsAndStones.LIB.Interfaces.Services.IO;

namespace PipsAndStones.Services.IO;

public class ConsoleLogger : IWriter, IReader
{
    public void Clear()
    {
        Console.Clear();
    }

    public void Write(string text)
    {
        Console.WriteLine(text);
    }

    public string Read()
    {
        return Console.ReadLine() ?? string.Empty;
    }
}