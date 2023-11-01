using System;
using System.IO;

namespace PCMonitor.Helpers;

public static class FileLogger
{
    private static readonly string _logFilePath = Path.Combine(Environment.CurrentDirectory, "log.txt");

    public static void Log(string message)
    {
        using (StreamWriter sw = File.AppendText(_logFilePath))
        {
            sw.WriteLine(message);
        }
    }
}
