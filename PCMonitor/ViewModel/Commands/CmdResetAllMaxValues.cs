using System;
using System.Windows.Input;

namespace PCMonitor.ViewModel.Commands;

public class CmdResetAllMaxValues : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public MainVM Vm { get; set; }

    public CmdResetAllMaxValues(MainVM vm)
    {
        Vm = vm;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var cmdResetSessionLoads = new CmdResetSessionLoads(Vm);
        cmdResetSessionLoads.Execute(this);

        var cmdResetSessionTemps = new CmdResetSessionTemperatures(Vm);
        cmdResetSessionTemps.Execute(this);
    }
}
