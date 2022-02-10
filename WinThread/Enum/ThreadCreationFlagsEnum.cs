/* ReSharper disable InconsistentNaming */

namespace WinKernel.WinThread.Enum;

public enum ThreadCreationFlagsEnum
{
    /// <summary>
    ///     The thread runs immediately after creation.
    /// </summary>
    RUN_AFTER_CREATION = 0x0,

    /// <summary>
    ///     The thread is created in a suspended state and does not run until the ResumeThread function is called.
    /// </summary>
    CREATE_SUSPENDED = 0x4,

    /// <summary>
    ///     The dwStackSize parameter specifies the initial reserve size of the stack. If this flag is not specified,
    ///     dwStackSize specifies the commit size.
    /// </summary>
    STACK_SIZE_PARAM_IS_A_RESERVATION = 0x00010000
}