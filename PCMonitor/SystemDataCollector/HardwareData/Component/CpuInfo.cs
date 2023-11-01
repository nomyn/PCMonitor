using LibreHardwareMonitor.Hardware;
using System;
using System.Linq;
using PCMonitor.Helpers;

namespace PCMonitor.SystemDataCollector.HardwareData.Component;

public class CpuInfo : IComponent
{
    public string? Name { get; set; }
    public float? Load { get; set; }
    public float? Temperature { get; set; }
    public float? Clock { get; set; }
    public float? Fan { get; set; }
    public string? MaxSpeed { get; set; }

    public void GetData(Computer computer)
    {
        try
        {
            var cpuInfo = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Cpu);
            if (cpuInfo != null)
            {
                Name = cpuInfo.Name;

                var smBiosProsessors = computer.SMBios.Processors.FirstOrDefault();
                if (smBiosProsessors != null)
                {
                    MaxSpeed = smBiosProsessors.MaxSpeed.ToString();
                }

                var tempSensors = cpuInfo.Sensors
                    .Where(s => s.SensorType == SensorType.Temperature)
                    .FirstOrDefault(s => s.Name.ToLower().Contains("core"));
                if (tempSensors != null)
                {
                    Temperature = tempSensors.Value;
                }

                var loadSensors = cpuInfo.Sensors
                    .FirstOrDefault(s => s.Name == "CPU Total");
                if (loadSensors != null)
                {
                    Load = loadSensors.Value;
                }

                var clockSensors = cpuInfo.Sensors
                    .Where(s => s.SensorType == SensorType.Clock)
                    .Where(s => s.Name.ToLower().Contains("core"));
                if (clockSensors != null)
                {
                    Clock = clockSensors.Average(s => s.Value);
                }

                var motherBoard = computer.Hardware.FirstOrDefault(h => h.HardwareType == HardwareType.Motherboard);
                if (motherBoard != null)
                {
                    var subHardware = motherBoard.SubHardware.FirstOrDefault();
                    if (subHardware != null)
                    {
                        var fanSensors = subHardware.Sensors.Where(s => s.SensorType == SensorType.Fan);
                        if (fanSensors.Any())
                        {
                            float? fan1 = 0;
                            float? fan2 = 0;
                            var fan1Sensor = fanSensors.FirstOrDefault(s => s.Name == "Fan #1");
                            if (fan1Sensor != null)
                            {
                                fan1 = fan1Sensor.Value;
                            }
                            var fan2Sensor = fanSensors.FirstOrDefault(s => s.Name == "Fan #2");
                            if (fan2Sensor != null)
                            {
                                fan2 = fan2Sensor.Value;
                            }
                            Fan = fan1 == 0 ? fan2 : fan1;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            FileLogger.Log("Error occured while getting CPU data.\n" + e.Message);
            throw;
        }
    }
}