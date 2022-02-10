using System.Runtime.InteropServices;

namespace WinKernel;

/// <summary>
///     P/Invoke for Windows kernel module <see cref="Kernel32" />
/// </summary>
internal static class Kernel32
{
    /// <summary>
    ///     ReadProcessMemory copies the data in the specified address range from the address space of the specified process
    ///     into the specified buffer of the current process. Any process that has a handle with PROCESS_VM_READ access can
    ///     call the function.
    ///     The entire area to be read must be accessible, and if it is not accessible, the function fails.
    /// </summary>
    /// <param name="hProcess">
    ///     A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ
    ///     access to the process.
    /// </param>
    /// <param name="lpBaseAddress">
    ///     A pointer to the base address in the specified process from which to read. Before any data
    ///     transfer occurs, the system verifies that all data in the base address and memory of the specified size is
    ///     accessible for read access, and if it is not accessible the function fails.
    /// </param>
    /// <param name="lpBuffer">
    ///     A pointer to a buffer that receives the contents from the address space of the specified
    ///     process.
    /// </param>
    /// <param name="dwSize">The number of bytes to be read from the specified process.</param>
    /// <param name="lpNumberOfBytesRead">
    ///     A pointer to a variable that receives the number of bytes transferred into the
    ///     specified buffer.
    /// </param>
    /// <returns>
    ///     If the function succeeds, the return value is nonzero.
    ///     If the function fails, the return value is 0 (zero). To get extended error information, call GetLastError.
    ///     The function fails if the requested read operation crosses into an area of the process that is
    ///     inaccessible.
    /// </returns>
    [DllImport("kernel32.dll")]
    internal static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [Out] byte[] lpBuffer,
        int dwSize,
        out IntPtr lpNumberOfBytesRead);

    /// <summary>
    ///     Writes data to an area of memory in a specified process. The entire area to be written to must be accessible or the
    ///     operation fails.
    /// </summary>
    /// <param name="hProcess">
    ///     A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and
    ///     PROCESS_VM_OPERATION access to the process.
    /// </param>
    /// <param name="lpBaseAddress">
    ///     A pointer to the base address in the specified process to which data is written. Before
    ///     data transfer occurs, the system verifies that all data in the base address and memory of the specified size is
    ///     accessible for write access, and if it is not accessible, the function fails.
    /// </param>
    /// <param name="lpBuffer">
    ///     A pointer to the buffer that contains data to be written in the address space of the specified
    ///     process.
    /// </param>
    /// <param name="dwSize">The number of bytes to be written to the specified process.</param>
    /// <param name="lpNumberOfBytesWritten">
    ///     A pointer to a variable that receives the number of bytes transferred into the
    ///     specified process. This parameter is optional. If lpNumberOfBytesWritten is NULL, the parameter is ignored.
    /// </param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    internal static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int dwSize,
        out int lpNumberOfBytesWritten);

    /// <summary>
    ///     This function returns a module handle for the specified module if the file has been mapped into the address space
    ///     of the calling process.
    /// </summary>
    /// <param name="lpModuleName">
    ///     Pointer to a null-terminated string that contains the name of the module, which must be  a .DLL file. If the
    ///     filename extension is omitted, the default library extension .DLL is appended. The filename string can include a
    ///     trailing point character (.) to indicate that the module name has no extension.
    ///     If this parameter is NULL, GetModuleHandle returns a handle to the file used to create the calling process.
    ///     All paths are ignored; only the file name and extension are used.
    ///     The file extensions .DLL and .CPL are treated as identical when comparing module names.
    /// </param>
    /// <returns>A handle to the specified module indicates success. NULL indicates failure.</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr GetModuleHandle(string lpModuleName);

    /// <summary>
    ///     Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
    /// </summary>
    /// <param name="hModule">A handle to the DLL module that contains the function or variable.</param>
    /// <param name="procName">
    ///     The function or variable name, or the function's ordinal value. If this parameter is an ordinal
    ///     value, it must be in the low-order word; the high-order word must be zero.
    /// </param>
    /// <returns>
    ///     If the function succeeds, the return value is the address of the exported function or variable.
    ///     If the function fails, the return value is NULL
    /// </returns>
    [DllImport("kernel32", CharSet = CharSet.Ansi)]
    internal static extern IntPtr GetProcAddress(
        IntPtr hModule,
        string procName);

    /// <summary>
    ///     Opens an existing local process object.
    /// </summary>
    /// <param name="dwDesiredAccess">
    ///     The access to the process object. This access right is checked against the security
    ///     descriptor for the process. T
    /// </param>
    /// <param name="bInheritHandle">
    ///     If this value is TRUE, processes created by this process will inherit the handle.
    ///     Otherwise, the processes do not inherit this handle.
    /// </param>
    /// <param name="dwProcessId">
    ///     The identifier of the local process to be opened.
    ///     If the specified process is the System Idle Process (0x00000000), the function fails and the last error code is
    ///     ERROR_INVALID_PARAMETER. If the specified process is the System process or one of the Client Server Run-Time
    ///     Subsystem (CSRSS) processes, this function fails and the last error code is ERROR_ACCESS_DENIED because their
    ///     access restrictions prevent user-level code from opening them.
    ///     If you are using GetCurrentProcessId as an argument to this function, consider using GetCurrentProcess instead of
    ///     OpenProcess, for improved performance.
    /// </param>
    /// <returns>
    ///     If the function succeeds, the return value is an open handle to the specified process.
    ///     If the function fails, the return value is NULL
    /// </returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    /// <summary>
    ///     Closes an open object handle.
    /// </summary>
    /// <param name="hProcess">A valid handle to an open object.</param>
    /// <returns>
    ///     If the function succeeds, the return value is nonzero.
    ///     If the function fails, the return value is zero.
    ///     If the application is running under a debugger, the function will throw an exception if it receives either a handle
    ///     value that is not valid or a pseudo-handle value. This can happen if you close a handle twice, or if you call
    ///     CloseHandle on a handle returned by the FindFirstFile function instead of calling the FindClose function.
    /// </returns>
    [DllImport("kernel32.dll")]
    internal static extern int CloseHandle(
        IntPtr hProcess);

    /// <summary>
    ///     Creates a thread that runs in the virtual address space of another process and optionally specifies extended
    ///     attributes such as processor group affinity.
    /// </summary>
    /// <param name="hProcess">
    ///     A handle to the process in which the thread is to be created. The handle must have the
    ///     PROCESS_CREATE_THREAD, PROCESS_QUERY_INFORMATION, PROCESS_VM_OPERATION, PROCESS_VM_WRITE, and PROCESS_VM_READ
    ///     access rights.
    /// </param>
    /// <param name="lpThreadAttributes">
    ///     A pointer to a SECURITY_ATTRIBUTES structure that specifies a security descriptor for
    ///     the new thread and determines whether child processes can inherit the returned handle. If lpThreadAttributes is
    ///     NULL, the thread gets a default security descriptor and the handle cannot be inherited. The access control lists
    ///     (ACL) in the default security descriptor for a thread come from the primary token of the creator.
    /// </param>
    /// <param name="dwStackSize">
    ///     The initial size of the stack, in bytes. The system rounds this value to the nearest page. If
    ///     this parameter is 0 (zero), the new thread uses the default size for the executable
    /// </param>
    /// <param name="lpStartAddress">
    ///     A pointer to the application-defined function of type LPTHREAD_START_ROUTINE to be
    ///     executed by the thread and represents the starting address of the thread in the remote process
    /// </param>
    /// <param name="lpParameter">
    ///     A pointer to a variable to be passed to the thread function pointed to by lpStartAddress.
    ///     This parameter can be NULL.
    /// </param>
    /// <param name="dwCreationFlags">The flags that control the creation of the thread.</param>
    /// <param name="lpThreadId">
    ///     A pointer to a variable that receives the thread identifier.
    ///     If this parameter is NULL, the thread identifier is not returned.
    /// </param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    internal static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        out IntPtr lpThreadId);

    /// <summary>
    ///     Retrieves the termination status of the specified process.
    /// </summary>
    /// <param name="hProcess">
    ///     A handle to the process.
    ///     The handle must have the PROCESS_QUERY_INFORMATION or PROCESS_QUERY_LIMITED_INFORMATION access right. For more
    ///     information, see Process Security and Access Rights.
    ///     Windows Server 2003 and Windows XP:  The handle must have the PROCESS_QUERY_INFORMATION access right.
    /// </param>
    /// <param name="exitCode">
    ///     A pointer to a variable to receive the process termination status. For more information, see
    ///     Remarks.
    /// </param>
    /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. </returns>
    /// <remarks>
    ///     This function returns immediately. If the process has not terminated and the function succeeds, the status
    ///     returned is STILL_ACTIVE (a macro for STATUS_PENDING (minwinbase.h))
    /// </remarks>
    [DllImport("kernel32.dll")]
    internal static extern bool GetExitCodeProcess(
        IntPtr hProcess,
        out int exitCode);

    /// <summary>
    ///     Changes the protection on a region of committed pages in the virtual address space of a specified process.
    /// </summary>
    /// <param name="hProcess">
    ///     A handle to the process whose memory protection is to be changed. The handle must have the
    ///     PROCESS_VM_OPERATION access right.
    /// </param>
    /// <param name="lpAddress">
    ///     A pointer to the base address of the region of pages whose access protection attributes are to be changed.
    ///     All pages in the specified region must be within the same reserved region allocated when calling the VirtualAlloc
    ///     or VirtualAllocEx function using MEM_RESERVE. The pages cannot span adjacent reserved regions that were allocated
    ///     by separate calls to VirtualAlloc or VirtualAllocEx using MEM_RESERVE.
    /// </param>
    /// <param name="dwSize">
    ///     The size of the region whose access protection attributes are changed, in bytes. The region of
    ///     affected pages includes all pages containing one or more bytes in the range from the lpAddress parameter to
    ///     (lpAddress+dwSize). This means that a 2-byte range straddling a page boundary causes the protection attributes of
    ///     both pages to be changed.
    /// </param>
    /// <param name="flNewProtect">The memory protection option. This parameter can be one of the memory protection constants.</param>
    /// <param name="lpflOldProtect">
    ///     A pointer to a variable that receives the previous access protection of the first page in
    ///     the specified region of pages. If this parameter is NULL or does not point to a valid variable, the function fails.
    /// </param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    internal static extern bool VirtualProtectEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        int dwSize,
        int flNewProtect,
        out int lpflOldProtect);

    /// <summary>
    ///     Reserves, commits, or changes the state of a region of memory within the virtual address space of a specified
    ///     process. The function initializes the memory it allocates to zero.
    /// </summary>
    /// <param name="hProcess">
    ///     The handle to a process. The function allocates memory within the virtual address space of this process.
    ///     The handle must have the PROCESS_VM_OPERATION access right.
    /// </param>
    /// <param name="lpAddress">
    ///     The pointer that specifies a desired starting address for the region of pages that you want to allocate.
    ///     If you are reserving memory, the function rounds this address down to the nearest multiple of the allocation
    ///     granularity.
    ///     If you are committing memory that is already reserved, the function rounds this address down to the nearest page
    ///     boundary. To determine the size of a page and the allocation granularity on the host computer, use the
    ///     GetSystemInfo function.
    ///     If lpAddress is NULL, the function determines where to allocate the region.
    ///     If this address is within an enclave that you have not initialized by calling InitializeEnclave, VirtualAllocEx
    ///     allocates a page of zeros for the enclave at that address. The page must be previously uncommitted, and will not be
    ///     measured with the EEXTEND instruction of the Intel Software Guard Extensions programming model.
    ///     If the address in within an enclave that you initialized, then the allocation operation fails with the
    ///     ERROR_INVALID_ADDRESS error
    /// </param>
    /// <param name="dwSize">
    ///     The size of the region of memory to allocate, in bytes.
    ///     If lpAddress is NULL, the function rounds dwSize up to the next page boundary.
    ///     If lpAddress is not NULL, the function allocates all pages that contain one or more bytes in the range from
    ///     lpAddress to lpAddress+dwSize. This means, for example, that a 2-byte range that straddles a page boundary causes
    ///     the function to allocate both pages.
    /// </param>
    /// <param name="flAllocationType">The type of memory allocation. This parameter must contain one of the following values.</param>
    /// <param name="flProtect">
    ///     The memory protection for the region of pages to be allocated. If the pages are being
    ///     committed, you can specify any one of the memory protection constants.
    /// </param>
    /// <returns>
    ///     If the function succeeds, the return value is the base address of the allocated region of pages.
    ///     If the function fails, the return value is NULL. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport("kernel32.dll")]
    internal static extern IntPtr VirtualAllocEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        uint flAllocationType,
        uint flProtect);
}