using App.Modules.Sys.Infrastructure.Services;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace App.Modules.Sys.Infrastructure.Services.Implementations;

/// <summary>
/// Production implementation of server device service.
/// Provides information about the server hosting the application.
/// </summary>
/// <remarks>
/// Singleton lifetime - server information doesn't change during runtime.
/// Uses .NET runtime APIs to gather system information.
/// </remarks>
public sealed class ServerDeviceService : IServerDeviceService
{
    private readonly string _machineName;
    private readonly string _operatingSystem;
    private readonly string _osVersion;
    private readonly int _processorCount;

    public ServerDeviceService()
    {
        _machineName = Environment.MachineName;
        _operatingSystem = GetOperatingSystemDescription();
        _osVersion = Environment.OSVersion.VersionString;
        _processorCount = Environment.ProcessorCount;
    }

    public string MachineName => _machineName;

    public string OperatingSystem => _operatingSystem;

    public string OsVersion => _osVersion;

    public string RuntimeVersion => RuntimeInformation.FrameworkDescription;

    public int ProcessorCount => _processorCount;

    public long TotalMemoryBytes => GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;

    public long UsedMemoryBytes => GC.GetTotalMemory(forceFullCollection: false);

    public double MemoryUsagePercent
    {
        get
        {
            var totalMemoryBytes = TotalMemoryBytes;
            if (totalMemoryBytes == 0)
            {
                return 0;
            }

            return (UsedMemoryBytes / (double)totalMemoryBytes) * 100.0;
        }
    }

    public TimeSpan Uptime
    {
        get
        {
            using var currentProcess = Process.GetCurrentProcess();
            return DateTime.UtcNow - currentProcess.StartTime.ToUniversalTime();
        }
    }

    public string GetServerInfo()
    {
        return $"{MachineName} | {OperatingSystem} {OsVersion} | {RuntimeVersion} | {ProcessorCount} CPUs";
    }

    public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;

    public bool Is64BitProcess => Environment.Is64BitProcess;

    public string ProcessArchitecture => RuntimeInformation.ProcessArchitecture.ToString();

    public string OsArchitecture => RuntimeInformation.OSArchitecture.ToString();

    private static string GetOperatingSystemDescription()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "Windows";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "Linux";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "macOS";
        }

        return "Unknown";
    }

    public long GetAvailableMemoryBytes()
    {
        var gcMemoryInfo = GC.GetGCMemoryInfo();
        return gcMemoryInfo.TotalAvailableMemoryBytes - UsedMemoryBytes;
    }

    public int GetThreadCount()
    {
        return Process.GetCurrentProcess().Threads.Count;
    }

    public long GetWorkingSetBytes()
    {
        return Environment.WorkingSet;
    }
}
