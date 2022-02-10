using System.Buffers.Binary;
using WinKernel.WinMemory.Enum;

namespace WinKernel.WinMemory;

public class WinMemoryReader
{
    private readonly IntPtr _processHandle;

    public WinMemoryReader(IntPtr processHandle)
    {
        _processHandle = processHandle;
    }
    
    public byte[] Read(IntPtr address, int size)
    {
        var data = new byte[size];

        Kernel32.VirtualProtectEx(_processHandle, address, size, (int) MemoryProtectionEnum.PAGE_EXECUTE_READWRITE,
            out int oldProtect);

        bool operationResult = Kernel32.ReadProcessMemory(_processHandle, address, data, size, out IntPtr read);

        Kernel32.VirtualProtectEx(_processHandle, address, size, oldProtect, out oldProtect);

        if (read != (IntPtr) size || !operationResult)
            throw new Exception(
                $"The reading at the address {address:X} read {read} byte(s) for a total of {size} byte(s) has failed !");

        return data;
    }

    public short ReadInt16(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(short));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadInt16BigEndian(data)
            : BinaryPrimitives.ReadInt16LittleEndian(data);
    }

    public int ReadInt32(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(int));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadInt32BigEndian(data)
            : BinaryPrimitives.ReadInt32LittleEndian(data);
    }

    public long ReadInt64(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(long));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadInt64BigEndian(data)
            : BinaryPrimitives.ReadInt64LittleEndian(data);
    }

    public ushort ReadUInt16(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(ushort));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadUInt16BigEndian(data)
            : BinaryPrimitives.ReadUInt16LittleEndian(data);
    }

    public uint ReadUInt32(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(uint));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadUInt32BigEndian(data)
            : BinaryPrimitives.ReadUInt32LittleEndian(data);
    }

    public ulong ReadUInt64(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(ulong));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadUInt64BigEndian(data)
            : BinaryPrimitives.ReadUInt64LittleEndian(data);
    }

    public float ReadFloat(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(float));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadSingleBigEndian(data)
            : BinaryPrimitives.ReadSingleLittleEndian(data);
    }

    public double ReadDouble(IntPtr address, EndiannessEnum endianness)
    {
        byte[] data = Read(address, sizeof(double));

        return endianness == EndiannessEnum.BigEndian
            ? BinaryPrimitives.ReadDoubleBigEndian(data)
            : BinaryPrimitives.ReadDoubleLittleEndian(data);
    }

    public bool ReadBoolean(IntPtr address, EndiannessEnum endiannessEnum)
    {
        return Read(address, sizeof(bool))[0] == 1;
    }
}