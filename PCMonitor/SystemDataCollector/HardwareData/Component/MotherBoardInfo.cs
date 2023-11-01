using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;
using PCMonitor.Helpers;

namespace PCMonitor.SystemDataCollector.HardwareData.Component;

public class MotherBoardInfo
{
    public string? Name { get; set; }

    public void GetData(Computer computer)
    {
        try
        {
            var mbInfo = computer.Hardware.FirstOrDefault(x => x.HardwareType == HardwareType.Motherboard);
            if (mbInfo != null)
            {
                Name = mbInfo.Name;
            }
            else
            {
                Name = "unableToGetMBName";
            }
        }
        catch (Exception e)
        {
            FileLogger.Log("Error occured while getting motherboard data.\n" + e.Message);
            throw;
        }
    }
}