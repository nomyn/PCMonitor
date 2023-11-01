using LibreHardwareMonitor.Hardware;

namespace PCMonitor.SystemDataCollector.HardwareData;

public static class HardwareInfo
{
    public static Computer CreateAndOpenComputer(
        bool batteryEnabled,
        bool controllerEnabled,
        bool cpuEnabled,
        bool gpuEnabled,
        bool memoryEnabled,
        bool mbEnabled,
        bool networkEnabled,
        bool psuEnabled,
        bool storageEnabled)
    {
        Computer computer = new Computer
        {
            IsBatteryEnabled = batteryEnabled,
            IsControllerEnabled = controllerEnabled,
            IsCpuEnabled = cpuEnabled,
            IsGpuEnabled = gpuEnabled,
            IsMemoryEnabled = memoryEnabled,
            IsMotherboardEnabled = mbEnabled,
            IsNetworkEnabled = networkEnabled,
            IsPsuEnabled = psuEnabled,
            IsStorageEnabled = storageEnabled
        };

        computer.Open();
        computer.Accept(new UpdateVisitor());

        return computer;
    }
}