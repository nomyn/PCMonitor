namespace PCMonitor.SystemDataCollector.HardwareData.Component;

public interface IComponent
{
    string? Name { get; set; }
    float? Load { get; set; }
    float? Temperature { get; set; }
    float? Clock { get; set; }
    float? Fan { get; set; }
}
