using System;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Infrastructure.Services;

/// <summary>
/// Server device/hardware information.
/// Provides runtime resource data for the server hosting the application.
/// </summary>
/// <remarks>
/// Lifetime: Singleton (server hardware doesn't change during runtime).
/// 
/// Use cases:
/// - Health checks (CPU/memory thresholds)
/// - Performance monitoring (resource utilization)
/// - Capacity planning (when to scale)
/// - Diagnostics (identify resource bottlenecks)
/// - Load balancing decisions
/// </remarks>
public interface IServerDeviceService: IHasService
{
    // ========================================
    // SERVER IDENTIFICATION
    // ========================================

    /// <summary>
    /// Server hostname (machine name).
    /// Example: "web-server-01", "ip-10-0-1-25"
    /// </summary>
    string HostName { get; }

    /// <summary>
    /// Fully qualified domain name (if available).
    /// Example: "web-server-01.production.mycompany.com"
    /// </summary>
    string? FullyQualifiedDomainName { get; }

    /// <summary>
    /// Server IP addresses (all network interfaces).
    /// </summary>
    string[] IpAddresses { get; }

    // ========================================
    // OPERATING SYSTEM
    // ========================================

    /// <summary>
    /// Operating system platform (Windows, Linux, macOS).
    /// </summary>
    string Platform { get; }

    /// <summary>
    /// Operating system version.
    /// Example: "Windows Server 2022", "Ubuntu 22.04 LTS"
    /// </summary>
    string OperatingSystemVersion { get; }

    /// <summary>
    /// OS architecture (x64, ARM64, etc.).
    /// </summary>
    string Architecture { get; }

    /// <summary>
    /// Whether running in containerized environment (Docker, Kubernetes, etc.).
    /// </summary>
    bool IsContainerized { get; }

    // ========================================
    // CPU RESOURCES
    // ========================================

    /// <summary>
    /// Number of logical processors (CPU cores).
    /// </summary>
    int ProcessorCount { get; }

    /// <summary>
    /// Current CPU usage percentage (0-100).
    /// </summary>
    /// <remarks>
    /// Expensive operation - polls system for current usage.
    /// Cache result if polling frequently.
    /// </remarks>
    double GetCurrentCpuUsagePercent();

    // ========================================
    // MEMORY RESOURCES
    // ========================================

    /// <summary>
    /// Total physical memory (RAM) in bytes.
    /// </summary>
    long TotalPhysicalMemoryBytes { get; }

    /// <summary>
    /// Total physical memory (RAM) in megabytes.
    /// </summary>
    long TotalPhysicalMemoryMB { get; }

    /// <summary>
    /// Total physical memory (RAM) in gigabytes.
    /// </summary>
    double TotalPhysicalMemoryGB { get; }

    /// <summary>
    /// Available (free) physical memory in bytes.
    /// </summary>
    /// <remarks>
    /// Expensive operation - polls system for current availability.
    /// Cache result if polling frequently.
    /// </remarks>
    long GetAvailablePhysicalMemoryBytes();

    /// <summary>
    /// Available (free) physical memory in megabytes.
    /// </summary>
    long GetAvailablePhysicalMemoryMB();

    /// <summary>
    /// Memory usage percentage (0-100).
    /// </summary>
    double GetMemoryUsagePercent();

    // ========================================
    // PROCESS RESOURCES
    // ========================================

    /// <summary>
    /// Current process ID.
    /// </summary>
    int CurrentProcessId { get; }

    /// <summary>
    /// Current process memory usage (working set) in bytes.
    /// </summary>
    long ProcessMemoryUsageBytes { get; }

    /// <summary>
    /// Current process memory usage (working set) in megabytes.
    /// </summary>
    long ProcessMemoryUsageMB { get; }

    /// <summary>
    /// Process uptime (how long application has been running).
    /// </summary>
    TimeSpan ProcessUptime { get; }

    /// <summary>
    /// When process (application) started (UTC).
    /// </summary>
    DateTime ProcessStartTimeUtc { get; }

    // ========================================
    // DISK RESOURCES
    // ========================================

    /// <summary>
    /// Total disk space in bytes (on drive containing application).
    /// </summary>
    long TotalDiskSpaceBytes { get; }

    /// <summary>
    /// Available (free) disk space in bytes.
    /// </summary>
    long AvailableDiskSpaceBytes { get; }

    /// <summary>
    /// Available disk space in gigabytes.
    /// </summary>
    double AvailableDiskSpaceGB { get; }

    /// <summary>
    /// Disk usage percentage (0-100).
    /// </summary>
    double DiskUsagePercent { get; }

    // ========================================
    // RUNTIME INFORMATION
    // ========================================

    /// <summary>
    /// .NET runtime version (e.g., ".NET 10.0.0").
    /// </summary>
    string RuntimeVersion { get; }

    /// <summary>
    /// Whether running in 64-bit process.
    /// </summary>
    bool Is64BitProcess { get; }

    /// <summary>
    /// Garbage collection mode (Workstation or Server).
    /// </summary>
    string GCMode { get; }

    // ========================================
    // HEALTH CHECK
    // ========================================

    /// <summary>
    /// Get overall server health status.
    /// Evaluates CPU, memory, disk thresholds.
    /// </summary>
    /// <returns>Server health status.</returns>
    ServerHealthStatus GetHealthStatus();
}

/// <summary>
/// Server health status.
/// </summary>
public record ServerHealthStatus
{
    /// <summary>
    /// Overall status (Healthy, Degraded, Unhealthy).
    /// </summary>
    public HealthStatus Status { get; init; }

    /// <summary>
    /// CPU usage percentage.
    /// </summary>
    public double CpuUsagePercent { get; init; }

    /// <summary>
    /// Memory usage percentage.
    /// </summary>
    public double MemoryUsagePercent { get; init; }

    /// <summary>
    /// Disk usage percentage.
    /// </summary>
    public double DiskUsagePercent { get; init; }

    /// <summary>
    /// Detailed status message.
    /// </summary>
    public string Message { get; init; } = null!;

    /// <summary>
    /// When status was evaluated (UTC).
    /// </summary>
    public DateTime CheckedAtUtc { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Health status enumeration.
/// </summary>
public enum HealthStatus
{
    /// <summary>
    /// All resources within acceptable thresholds.
    /// </summary>
    Healthy = 0,

    /// <summary>
    /// One or more resources approaching thresholds (warning).
    /// </summary>
    Degraded = 1,

    /// <summary>
    /// One or more resources exceeded thresholds (critical).
    /// </summary>
    Unhealthy = 2
}
