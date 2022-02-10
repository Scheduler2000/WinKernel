using System.Diagnostics;
using System.Text;
using WinKernel.WinMemory;
using WinKernel.WinMemory.Enum;
using WinKernel.WinProcess.Enum;
using WinKernel.WinThread.Enum;

namespace WinKernel.WinProcess;

public sealed class WinProcess : IDisposable
{
    private readonly Process _process;

    public WinMemoryWriter ProcessWriter { get; }

    public WinMemoryReader ProcessReader { get; }

    public bool IsActive
    {
        get
        {
            if (!Kernel32.GetExitCodeProcess(_process.Handle, out int value)) return false;
            return value == 0x00000103;
        }
    }

    public WinProcess(Process process)
    {
        _process = process;

        ProcessWriter = new WinMemoryWriter(_process.Handle);
        ProcessReader = new WinMemoryReader(_process.Handle);
    }


    public ProcessModule FetchProcessModule(string moduleName)
    {
        foreach (ProcessModule processModule in _process.Modules)
            if (processModule.ModuleName == moduleName)
                return processModule;

        return null;
    }

    public bool InjectDll(string dll)
    {
        IntPtr procHandle =
            Kernel32.OpenProcess(
                (int) ProcessSecurityEnum.ALL,
                false, _process.Id);

        if (procHandle == IntPtr.Zero) return false;

        byte[] dllBin = Encoding.ASCII.GetBytes(dll);

        IntPtr dllAddress = Kernel32.VirtualAllocEx(procHandle,
            IntPtr.Zero,
            (uint) dllBin.Length,
            (uint) (AllocationTypeEnum.MEM_COMMIT | AllocationTypeEnum.MEM_RESERVE),
            (uint) MemoryProtectionEnum.PAGE_READWRITE);

        if (dllAddress == IntPtr.Zero) return false;
        
        Kernel32.VirtualProtectEx(procHandle,
            dllAddress,
            dll.Length,
            (int) MemoryProtectionEnum.PAGE_EXECUTE_READWRITE,
            out int oldProtect);

        bool operationResult = Kernel32.WriteProcessMemory(procHandle,
            dllAddress,
            dllBin,
            dll.Length,
            out int bytesWritten);

        if (!operationResult || bytesWritten == 0) return false;

        Kernel32.VirtualProtectEx(procHandle,
            dllAddress,
            dllBin.Length,
            oldProtect,
            out _);

        IntPtr loadedFunc = Kernel32.GetProcAddress(Kernel32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

        if (loadedFunc == IntPtr.Zero) return false;

        IntPtr handleThread = Kernel32.CreateRemoteThread(procHandle,
            IntPtr.Zero,
            0,
            loadedFunc,
            dllAddress,
            (uint) ThreadCreationFlagsEnum.RUN_AFTER_CREATION,
            out _);

        if (handleThread == IntPtr.Zero) return false;

        Kernel32.CloseHandle(handleThread);
        return true;
    }
    
    public void Dispose()
    {
        if (Kernel32.CloseHandle(_process.Handle) == 0)
            throw new Exception(
                $"The closing of the handle {_process.Handle} failed for the process {_process.ProcessName} (pid={_process.Id})");
    }
}