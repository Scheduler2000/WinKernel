# WinKernel

Lightweight library to interfere with windows kernel written in .net 6

## Features : 
  - Binaries Reading in big endian  (see `WinMemory\WinMemoryReader.cs`)
  - Binaries Reading in litle endian (see `WinMemory\WinMemoryReader.cs`)
  - Binaries Writing in big endian (see `WinMemory\WinMemoryWriter.cs`)
  - Binaries Writing in little endian  (see `WinMemory\WinMemoryWriter.cs`)
  - Native dll injection (see `WinProcess\WinProcess.cs`) for x86 and x64 processes


 ## Example of Dll Injection
 ```cs 
    var dllPath = "path_to_my_dll";
    
    var targetedProcessName = "process_name";
    
    var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == targetedProcessName);
      
    using WinProcess winProcess = new(process); 

    Console.WriteLine(winProcess.InjectDll(dllPath)? "Injectaion dll succeed" : "An error has occured during the dll injection.");
 ```
 
 ## Example Reading and Writing in Memory
 
 ```cs
    var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "ac_client"); /* assault_cube.exe x86 */

    using WinProcess winProcess = new(process);
    var processModule = winProcess.FetchProcessModule("ac_client.exe");


    IntPtr playerPtr = new(winProcess.ProcessReader.ReadInt32(processModule.BaseAddress + 0x10F4F4, EndiannessEnum.LittleEndian));

    var playerHealth = winProcess.ProcessReader.ReadInt32(playerPtr + 0xF8, EndiannessEnum.LittleEndian);
    var playerAmmo = winProcess.ProcessReader.ReadInt32(playerPtr + 0x150, EndiannessEnum.LittleEndian);
    var playerPosX = winProcess.ProcessReader.ReadFloat(playerPtr + 0x4, EndiannessEnum.LittleEndian);
    var playerPosY = winProcess.ProcessReader.ReadFloat(playerPtr + 0x8, EndiannessEnum.LittleEndian);
    var playerPosZ = winProcess.ProcessReader.ReadFloat(playerPtr + 0xC, EndiannessEnum.LittleEndian);

    playerHealth = 5000;
    playerAmmo = 999;
    playerPosX = 200.05f;
    playerPosY = 180.541f;
    playerPosZ = 400.00f;

    winProcess.ProcessWriter.WriteInt32(playerPtr + 0xF8, playerHealth, EndiannessEnum.LittleEndian);
    winProcess.ProcessWriter.WriteInt32(playerPtr + 0x150, playerAmmo, EndiannessEnum.LittleEndian);
    winProcess.ProcessWriter.WriteFloat(playerPtr + 0x4, playerPosX, EndiannessEnum.LittleEndian);
    winProcess.ProcessWriter.WriteFloat(playerPtr + 0x8, playerPosY, EndiannessEnum.LittleEndian);
    winProcess.ProcessWriter.WriteFloat(playerPtr + 0xC, playerPosZ, EndiannessEnum.LittleEndian);
 ```
