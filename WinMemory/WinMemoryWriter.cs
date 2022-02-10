using System.Buffers.Binary;
using WinKernel.WinMemory.Enum;

namespace WinKernel.WinMemory;

public class WinMemoryWriter
{
    private readonly IntPtr _processHandle;

    public WinMemoryWriter(IntPtr processHandle)
    {
        _processHandle = processHandle;
    }

    public void Write(IntPtr address, byte[] data)
    {
        Kernel32.VirtualProtectEx(_processHandle, address, data.Length, (int) MemoryProtectionEnum.PAGE_EXECUTE_READWRITE,
            out int oldProtect);

        bool operationResult = Kernel32.WriteProcessMemory(_processHandle, address, data, data.Length, out int bytesWritten);

        Kernel32.VirtualProtectEx(_processHandle, address, data.Length, oldProtect,
            out oldProtect);

        if (bytesWritten != data.Length || !operationResult)
            throw new Exception(
                $"The writing at the address {address:X} wrote {bytesWritten} byte(s) for a total of {data.Length} byte(s) has failed !");
    }

    public void WriteInt16(IntPtr address, short value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(short)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteInt16LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteInt16BigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteInt32(IntPtr address, int value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteInt32LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteInt32BigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteInt64(IntPtr address, long value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteInt64BigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteUInt16(IntPtr address, ushort value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteUInt16LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteUInt16BigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteUInt32(IntPtr address, uint value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteUInt32LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteUInt32BigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteUInt64(IntPtr address, ulong value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteUInt64BigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteFloat(IntPtr address, float value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(float)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteSingleLittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteSingleBigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteDouble(IntPtr address, double value, EndiannessEnum endianness)
    {
        Span<byte> buffer = stackalloc byte[sizeof(double)];

        if (endianness == EndiannessEnum.LittleEndian)
            BinaryPrimitives.WriteDoubleLittleEndian(buffer, value);
        else
            BinaryPrimitives.WriteDoubleBigEndian(buffer, value);

        Write(address, buffer.ToArray());
    }

    public void WriteBoolean(IntPtr address, bool value)
    {
        Write(address, new[] {(byte) (value ? 1 : 0)});
    }
}