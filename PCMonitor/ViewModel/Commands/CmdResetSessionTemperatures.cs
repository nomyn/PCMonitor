using System;
using System.Windows.Input;

namespace PCMonitor.ViewModel.Commands;

public class CmdResetSessionTemperatures : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public MainVM Vm { get; set; }

    public CmdResetSessionTemperatures(MainVM vm)
    {
        Vm = vm;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        Vm.SessionTopCpuTemp = 0;
        Vm.SessionTopGpuTemp = 0;
    }
}
