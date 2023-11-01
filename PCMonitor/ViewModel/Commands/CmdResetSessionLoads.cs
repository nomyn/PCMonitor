using System;
using System.Windows.Input;

namespace PCMonitor.ViewModel.Commands;

public class CmdResetSessionLoads : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public MainVM Vm { get; set; }

    public CmdResetSessionLoads(MainVM vm)
    {
        Vm = vm;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        Vm.SessionTopCpuLoad = 0;
        Vm.SessionTopGpuLoad = 0;
    }
}
