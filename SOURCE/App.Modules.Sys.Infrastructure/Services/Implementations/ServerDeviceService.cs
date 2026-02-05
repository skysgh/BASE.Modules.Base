using App.Modules.Sys.Infrastructure.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime;
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
    private readonly string _hostName;
    private readonly string? _fqdn;
    private readonly string[] _ipAddresses;
    private readonly string _platform;
    private readonly string _osVersion;
    private readonly string _architecture;
    private readonly int _processorCount;
    private readonly long _totalMemoryBytes;
    private readonly int _processId;
    private readonly DateTime _processStartTimeUtc;
    private readonly string _runtimeVersion;
    private readonly bool _is64BitProcess;
    private readonly string _gcMode;
    private readonly long _totalDiskSpaceBytes;
    private readonly DriveInfo _appDrive;

    public ServerDeviceService()
    {
        _hostName = Environment.MachineName;
        _fqdn = GetFullyQualifiedDomainName();
        _ipAddresses = GetLocalIpAddresses();
        _platform = GetPlatform();
        _osVersion = Environment.OSVersion.VersionString;
        _architecture = RuntimeInformation.OSArchitecture.ToString();
        _processorCount = Environment.ProcessorCount;
        _totalMemoryBytes = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
        
        var process = Process.GetCurrentProcess();
        _processId = process.Id;
        _processStartTimeUtc = process.StartTime.ToUniversalTime();
        
        _runtimeVersion = RuntimeInformation.FrameworkDescription;
        _is64BitProcess = Environment.Is64BitProcess;
        _gcMode = GCSettings.IsServerGC ? "Server" : "Workstation";
        
        _appDrive = new DriveInfo(AppContext.BaseDirectory);
        _totalDiskSpaceBytes = _appDrive.TotalSize;
    }

    // ========================================
    // SERVER IDENTIFICATION
    // ========================================

    public string HostName => _hostName;

    public string? FullyQualifiedDomainName => _fqdn;

    public string[] IpAddresses => _ipAddresses;

    // ========================================
    // OPERATING SYSTEM
    // ========================================

    public string Platform => _platform;

    public string OperatingSystemVersion => _osVersion;

    public string Architecture => _architecture;

    public bool IsContainerized
    {
        get
        {
            // Check for Docker environment variable
            var inDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
            if (!string.IsNullOrEmpty(inDocker))
            {
                return true;
            }

            // Check for Kubernetes
            var kubernetesService = Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST");
            if (!string.IsNullOrEmpty(kubernetesService))
            {
                return true;
            }

            // Check for /.dockerenv file (Linux containers)
            if (File.Exists("/.dockerenv"))
            {
                return true;
            }

            return false;
        }
    }

    // ========================================
    // CPU RESOURCES
    // ========================================

    public int ProcessorCount => _processorCount;

    public double GetCurrentCpuUsagePercent()
    {
        // Simple approximation - would need performance counters for accurate measurement
        using var process = Process.GetCurrentProcess();
        var startTime = DateTime.UtcNow;
        var startCpuUsage = process.TotalProcessorTime;

        System.Threading.Thread.Sleep(500); // Sample period

        var endTime = DateTime.UtcNow;
        var endCpuUsage = process.TotalProcessorTime;

        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
        var totalMsPassed = (endTime - startTime).TotalMilliseconds;
        var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

        return cpuUsageTotal * 100;
    }

    // ========================================
    // MEMORY RESOURCES
    // ========================================

    public long TotalPhysicalMemoryBytes => _totalMemoryBytes;

    public long TotalPhysicalMemoryMB => _totalMemoryBytes / (1024 * 1024);

    public double TotalPhysicalMemoryGB => _totalMemoryBytes / (1024.0 * 1024.0 * 1024.0);

    public long GetAvailablePhysicalMemoryBytes()
    {
        var gcMemoryInfo = GC.GetGCMemoryInfo();
        return gcMemoryInfo.TotalAvailableMemoryBytes - GC.GetTotalMemory(forceFullCollection: false);
    }

    public long GetAvailablePhysicalMemoryMB()
    {
        return GetAvailablePhysicalMemoryBytes() / (1024 * 1024);
    }

    public double GetMemoryUsagePercent()
    {
        var used = GC.GetTotalMemory(forceFullCollection: false);
        var total = _totalMemoryBytes;
        if (total == 0)
        {
            return 0;
        }
        return (used / (double)total) * 100.0;
    }

    // ========================================
    // PROCESS RESOURCES
    // ========================================

    public int CurrentProcessId => _processId;

    public long ProcessMemoryUsageBytes
    {
        get
        {
            using var process = Process.GetCurrentProcess();
            return process.WorkingSet64;
        }
    }

    public long ProcessMemoryUsageMB => ProcessMemoryUsageBytes / (1024 * 1024);

    public TimeSpan ProcessUptime => DateTime.UtcNow - _processStartTimeUtc;

    public DateTime ProcessStartTimeUtc => _processStartTimeUtc;

    // ========================================
    // DISK RESOURCES
    // ========================================

    public long TotalDiskSpaceBytes => _totalDiskSpaceBytes;

    public long AvailableDiskSpaceBytes => _appDrive.AvailableFreeSpace;

    public double AvailableDiskSpaceGB => AvailableDiskSpaceBytes / (1024.0 * 1024.0 * 1024.0);

    public double DiskUsagePercent
    {
        get
        {
            var used = _totalDiskSpaceBytes - AvailableDiskSpaceBytes;
            if (_totalDiskSpaceBytes == 0)
            {
                return 0;
            }
            return (used / (double)_totalDiskSpaceBytes) * 100.0;
        }
    }

    // ========================================
    // RUNTIME INFORMATION
    // ========================================

    public string RuntimeVersion => _runtimeVersion;

    public bool Is64BitProcess => _is64BitProcess;

    public string GCMode => _gcMode;

    // ========================================
    // HEALTH CHECK
    // ========================================

    public ServerHealthStatus GetHealthStatus()
    {
        var cpuPercent = 0.0; // Would need actual measurement - expensive operation
        var memoryPercent = GetMemoryUsagePercent();
        var diskPercent = DiskUsagePercent;

        HealthStatus status;
        string message;

        if (memoryPercent > 90 || diskPercent > 90)
        {
            status = HealthStatus.Unhealthy;
            message = $"Critical: Memory at {memoryPercent:F1}%, Disk at {diskPercent:F1}%";
        }
        else if (memoryPercent > 75 || diskPercent > 75)
        {
            status = HealthStatus.Degraded;
            message = $"Warning: Memory at {memoryPercent:F1}%, Disk at {diskPercent:F1}%";
        }
        else
        {
            status = HealthStatus.Healthy;
            message = "All resources within acceptable limits";
        }

        return new ServerHealthStatus
        {
            Status = status,
            CpuUsagePercent = cpuPercent,
            MemoryUsagePercent = memoryPercent,
            DiskUsagePercent = diskPercent,
            Message = message,
            CheckedAtUtc = DateTime.UtcNow
        };
    }

    // ========================================
    // PRIVATE HELPERS
    // ========================================

    private static string GetPlatform()
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

    private static string? GetFullyQualifiedDomainName()
    {
        try
        {
            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            return hostEntry.HostName;
        }
        catch
        {
            return null;
        }
    }

    private static string[] GetLocalIpAddresses()
    {
        try
        {
            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            
            return hostEntry.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork || 
                            ip.AddressFamily == AddressFamily.InterNetworkV6)
                .Select(ip => ip.ToString())
                .ToArray();
        }
        catch
        {
            return Array.Empty<string>();
        }
    }
}

