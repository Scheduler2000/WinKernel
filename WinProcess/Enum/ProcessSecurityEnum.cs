/* ReSharper disable InconsistentNaming */

namespace WinKernel.WinProcess.Enum;

[Flags]
public enum ProcessSecurityEnum
{
    ALL = 0x001F0FFF,
    PROCESS_CREATE_THREAD = 0x0002,
    PROCESS_QUERY_INFORMATION = 0x0400,
    PROCESS_VM_OPERATION = 0x0008,
    PROCESS_VM_WRITE = 0x0020,
    PROCESS_VM_READ = 0x0010
}