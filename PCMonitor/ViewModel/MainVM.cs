using LibreHardwareMonitor.Hardware;
using System.ComponentModel;
using System.Windows.Threading;
using PCMonitor.ViewModel.Commands;
using System;
using PCMonitor.SystemDataCollector.EnvironmentData;
using PCMonitor.SystemDataCollector.HardwareData;
using PCMonitor.SystemDataCollector.HardwareData.Component;

namespace PCMonitor.ViewModel;

public class MainVM : INotifyPropertyChanged
{
    // eventhandlers
    public event PropertyChangedEventHandler? PropertyChanged;

    // variables
    private DispatcherTimer _updateTimer;
    // make parameters below app options
    private Computer _myComputer = HardwareInfo.CreateAndOpenComputer(
            batteryEnabled: false,
            controllerEnabled: true,
            cpuEnabled: true,
            gpuEnabled: true,
            memoryEnabled: true,
            mbEnabled: true,
            networkEnabled: false,
            psuEnabled: false,
            storageEnabled: false);

    // properties
    public CmdResetSessionLoads CmdResetSessionLoads { get; set; }
    public CmdResetSessionTemperatures CmdResetSessionTemperatures { get; set; }
    public CmdResetAllMaxValues CmdResetAllMaxValues { get; set; }
    public float? SessionTopCpuTemp { get; set; } = 0;
    public float? SessionTopCpuLoad { get; set; } = 0;
    public float? SessionTopGpuTemp { get; set; } = 0;
    public float? SessionTopGpuLoad { get; set; } = 0;

    // constructor
    public MainVM()
    {
        // commands
        CmdResetSessionLoads = new CmdResetSessionLoads(this);
        CmdResetSessionTemperatures = new CmdResetSessionTemperatures(this);
        CmdResetAllMaxValues = new CmdResetAllMaxValues(this);

        // initial setup
        var envInfo = new EnvironmentInfo();
        envInfo.GetData();
        if (envInfo != null)
        {
            AssignEnvironmentInfoToProperties(envInfo);
        }
        AssingOrUpdateHardwareInfoToProperties(_myComputer);
        // dispatcher setup
        _updateTimer = new DispatcherTimer();
        _updateTimer.Tick += new EventHandler(UpdateTimer_Tick);
        // interval in options
        _updateTimer.Interval = new TimeSpan(0, 0, 1);
        _updateTimer.Start();
    }

    // propfulls
    private string? osType = default;
    public string OsType
    {
        get { return osType; }
        set { osType = value; }
    }

    private string? machineName = default;
    public string MachineName
    {
        get { return machineName; }
        set { machineName = value; }
    }

    private string? userName = default;
    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }

    private string? motherBoardName = "";
    public string MotherBoardName
    {
        get { return motherBoardName; }
        set { motherBoardName = value; }
    }

    private string? cpuType = default;
    public string CpuType
    {
        get { return cpuType; }
        set
        {
            cpuType = value;
            OnPropertyChanged("CpuType");
        }
    }

    private string? cpuLoad;
    public string? CpuLoad
    {
        get { return cpuLoad; }
        set 
        { 
            cpuLoad = value;
            OnPropertyChanged("CpuLoad");
        }
    }

    private string? cpuTemp;
    public string? CpuTemp
    {
        get { return cpuTemp; }
        set
        {
            cpuTemp = value;
            OnPropertyChanged("CpuTemp");
        }
    }

    private string? cpuClock;
    public string? CpuClock
    {
        get { return cpuClock; }
        set
        {
            cpuClock = value;
            OnPropertyChanged("CpuClock");
        }
    }

    private string? cpuFan;
    public string? CpuFan
    {
        get { return cpuFan; }
        set
        {
            cpuFan = value;
            OnPropertyChanged("CpuFan");
        }
    }

    private string? gpuType = default;
    public string GpuType
    {
        get { return gpuType; }
        set
        {
            gpuType = value;
            OnPropertyChanged("GpuType");
        }
    }

    private string? gpuLoad;
    public string? GpuLoad
    {
        get { return gpuLoad; }
        set
        {
            gpuLoad = value;
            OnPropertyChanged("GpuLoad");
        }
    }

    private string? gpuTemp;
    public string? GpuTemp
    {
        get { return gpuTemp; }
        set
        {
            gpuTemp = value;
            OnPropertyChanged("GpuTemp");
        }
    }

    private string? gpuClock;
    public string? GpuClock
    {
        get { return gpuClock; }
        set
        {
            gpuClock = value;
            OnPropertyChanged("GpuClock");
        }
    }

    private string? gpuFan;
    public string? GpuFan
    {
        get { return gpuFan; }
        set
        {
            gpuFan = value;
            OnPropertyChanged("GpuFan");
        }
    }

    private string gpuMemory;
    public string GpuMemory
    {
        get { return gpuMemory; }
        set 
        {
            gpuMemory = value;
            OnPropertyChanged("GpuMemory");
        }
    }

    private string ramInfo;
    public string RamInfo
    {
        get { return ramInfo; }
        set
        {
            ramInfo = value;
            OnPropertyChanged("RamInfo");
        }
    }

    private string ramTotal;
    public string RamTotal
    {
        get { return ramTotal; }
        set
        {
            ramTotal = value;
            OnPropertyChanged("RamTotal");
        }
    }

    private string ramUsed;
    public string RamUsed
    {
        get { return ramUsed; }
        set
        {
            ramUsed = value;
            OnPropertyChanged("RamUsed");
        }
    }

    // methods
    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void UpdateTimer_Tick(object sender, EventArgs e) => AssingOrUpdateHardwareInfoToProperties(_myComputer);

    private void AssignEnvironmentInfoToProperties(EnvironmentInfo envInfo)
    {
        OsType = envInfo.OperatingSystemType;
        UserName = envInfo.UserName;
        MachineName = envInfo.MachineName;
    }

    private void AssingOrUpdateHardwareInfoToProperties(Computer computer)
    {
        if (computer != null)
        {
            // always update hardware for latest data
            computer.Accept(new UpdateVisitor());

            // assing mb only once
            if (MotherBoardName == "")
            {
                var mbInfo = new MotherBoardInfo();
                mbInfo.GetData(computer);
                MotherBoardName = mbInfo.Name;
            }

            // cpu data
            if (computer.IsCpuEnabled)
            {
                var cpuInfo = new CpuInfo();
                cpuInfo.GetData(computer);
                if (cpuInfo != null)
                {
                    CpuType = cpuInfo.Name;
                    var temp = cpuInfo.Temperature;
                    if (temp > SessionTopCpuTemp)
                        SessionTopCpuTemp = temp;
                    CpuTemp = $"{temp:0.##}°C (Max. reached {SessionTopCpuTemp:0.##}°C)";
                    var load = cpuInfo.Load;
                    if (load > SessionTopCpuLoad)
                        SessionTopCpuLoad = load;
                    CpuLoad = $"{load:0.##}% (Max. reached {SessionTopCpuLoad:0.##}%)";
                    CpuClock = $"{cpuInfo.Clock:0.##} MHz (Limit @ {cpuInfo.MaxSpeed} MHz)";
                    CpuFan = $"{cpuInfo.Fan:#} RPM";
                }
            }

            // gpu data
            if (computer.IsGpuEnabled)
            {
                var gpuInfo = new GpuInfo();
                gpuInfo.GetData(computer);
                if (gpuInfo != null)
                {
                    GpuType = gpuInfo.Name;
                    var temp = gpuInfo.Temperature;
                    if (temp > SessionTopGpuTemp)
                        SessionTopGpuTemp = temp;
                    GpuTemp = $"{temp:0.##}°C (Max. reached {SessionTopGpuTemp:0.##}°C)";
                    var load = gpuInfo.Load;
                    if (load > SessionTopGpuLoad)
                        SessionTopGpuLoad = load;
                    GpuLoad = $"{load:0.##}% (Max. reached {SessionTopGpuLoad:0.##}%)";
                    GpuClock = $"{gpuInfo.Clock:0.##} MHz";
                    GpuFan = $"{gpuInfo.Fan} RPM";
                    var memoryString = $"{gpuInfo.UsedMemory}MB / {gpuInfo.TotalMemory - gpuInfo.UsedMemory}MB (Used {gpuInfo.UsedMemory / gpuInfo.TotalMemory * 100:0.##}%, total {gpuInfo.TotalMemory}MB)";
                    GpuMemory = memoryString;
                }
            }

            // ram data
            if (computer.IsMemoryEnabled)
            {
                var ramInfo = new RamInfo();
                ramInfo.GetData(computer);
                if (ramInfo != null)
                {
                    RamInfo = $"{ramInfo.RamCount} X {ramInfo.RamType} {ramInfo.RamSpeed}MHz {ramInfo.RamSize}MB";
                    RamTotal = $"{ramInfo.RamTotalSize}GB";
                    RamUsed = $"{ramInfo.RamUsed:0.##}GB / {ramInfo.RamFree:0.##}GB (Used {ramInfo.RamUsed / ramInfo.RamTotalSize * 100:0.##}%)";
                }
            }
        }
    }
}
