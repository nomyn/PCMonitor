using System;

namespace PCMonitor.SystemDataCollector.EnvironmentData;

public class EnvironmentInfo
{
    public string? OperatingSystemType { get; set; }
    public string? MachineName { get; set; }
    public string? UserName { get; set; }

    public void GetData()
    {
        OperatingSystem os = Environment.OSVersion;
        OperatingSystemType = os.VersionString switch
        {
            "Microsoft Windows NT 10.0.22621.0" => $"Windows 11 ({os.VersionString})",
            _ => $"{os.VersionString}"
        };
        MachineName = Environment.MachineName;
        UserName = Environment.UserName;
    }
}