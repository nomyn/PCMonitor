using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;
using PCMonitor.Helpers;

namespace PCMonitor.SystemDataCollector.HardwareData.Component;

public class RamInfo
{
    public int RamCount { get; set; }
    public MemoryType RamType { get; set; }
    public ushort RamSpeed { get; set; }
    public ushort RamSize { get; set; }
    public int RamTotalSize { get; set; }
    public float? RamUsed { get; set; }
    public float? RamFree { get; set; }

    public void GetData(Computer computer)
    {
        try
        {
            var memoryDevices = computer.SMBios.MemoryDevices;
            if (memoryDevices != null)
            {
                RamCount = memoryDevices.Length;
                RamType = memoryDevices[0].Type;
                RamSpeed = memoryDevices[0].Speed;
                RamSize = memoryDevices[0].Size;
                RamTotalSize = memoryDevices.Sum(s => s.Size) / 1024; // Convert to GB
            }

            var memoryHardware = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Memory);
            if (memoryHardware != null)
            {
                var ramSensors = memoryHardware.Sensors.Where(s => s.SensorType == SensorType.Data);
                if (ramSensors != null)
                {
                    var ramSensor = ramSensors.FirstOrDefault(s => s.Name == "Memory Used");
                    if (ramSensor != null)
                    {
                        RamUsed = ramSensor.Value;
                        RamFree = RamTotalSize - RamUsed;
                    }
                }
            }
        }
        catch (Exception e)
        {
            FileLogger.Log("Error occured while getting RAM data.\n" + e.Message);
            throw;
        }
    }
}
