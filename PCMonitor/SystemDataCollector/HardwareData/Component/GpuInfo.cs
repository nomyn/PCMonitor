using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;
using PCMonitor.Helpers;

namespace PCMonitor.SystemDataCollector.HardwareData.Component;

public class GpuInfo : IComponent
{
    public string? Name { get; set; }
    public float? Load { get; set; }
    public float? Temperature { get; set; }
    public float? Clock { get; set; }
    public float? Fan { get; set; }
    public float? TotalMemory { get; set; }
    public float? UsedMemory { get; set; }

    public void GetData(Computer computer)
    {
        try
        {
            var gpuInfo = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.GpuNvidia || h.HardwareType == HardwareType.GpuAmd);
            if (gpuInfo != null)
            {
                Name = gpuInfo.Name;

                var tempSensors = gpuInfo.Sensors
                    .Where(s => s.SensorType == SensorType.Temperature)
                    .FirstOrDefault(s => s.Name.ToLower().Contains("core"));
                if (tempSensors != null)
                {
                    Temperature = tempSensors.Value;
                }

                var loadSensors = gpuInfo.Sensors
                    .Where(s => s.SensorType == SensorType.Load)
                    .FirstOrDefault(s => s.Name.ToLower().Contains("core"));
                if (loadSensors != null)
                {
                    Load = loadSensors.Value;
                }

                var clockSensors = gpuInfo.Sensors
                    .Where(s => s.SensorType == SensorType.Clock)
                    .FirstOrDefault(s => s.Name.ToLower().Contains("core"));
                if (clockSensors != null)
                {
                    Clock = clockSensors.Value;
                }

                var fanSensor = gpuInfo.Sensors
                    .FirstOrDefault(s => s.Name == "GPU Fan");
                if (fanSensor != null)
                {
                    Fan = fanSensor.Value;
                }

                var memorySensors = gpuInfo.Sensors
                    .Where(s => s.SensorType == SensorType.SmallData);
                if (memorySensors != null)
                {
                    var totalMemorySensor = memorySensors.FirstOrDefault(s => s.Name == "GPU Memory Total");
                    if (totalMemorySensor != null)
                    {
                        TotalMemory = totalMemorySensor.Value;
                    }
                    var usedMemorySensor = memorySensors.FirstOrDefault(s => s.Name == "GPU Memory Used");
                    if (usedMemorySensor != null)
                    {
                        UsedMemory = usedMemorySensor.Value;
                    }
                }
            }
        }
        catch (Exception e)
        {
            FileLogger.Log("Error occured while getting GPU data.\n" + e.Message);
            throw;
        }
    }
}