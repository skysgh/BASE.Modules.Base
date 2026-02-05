using System;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Infrastructure.Services;

/// <summary>
/// Client device information (parsed from User-Agent and request headers).
/// Provides details about the device/browser making the request.
/// </summary>
/// <remarks>
/// Lifetime: Scoped (per request - parsed from User-Agent header).
/// 
/// Use cases:
/// - Responsive design hints (serve mobile vs desktop layouts)
/// - Feature detection (check browser capabilities)
/// - Analytics (track device/browser usage)
/// - Security (block outdated/insecure browsers)
/// - Device-specific optimizations
/// </remarks>
public interface IClientDeviceService: IHasService
{
    // ========================================
    // DEVICE TYPE
    // ========================================

    /// <summary>
    /// Device type category (Desktop, Mobile, Tablet, Bot, Unknown).
    /// </summary>
    DeviceType DeviceType { get; }

    /// <summary>
    /// Whether request is from mobile device (phone).
    /// </summary>
    bool IsMobile { get; }

    /// <summary>
    /// Whether request is from tablet device.
    /// </summary>
    bool IsTablet { get; }

    /// <summary>
    /// Whether request is from desktop/laptop device.
    /// </summary>
    bool IsDesktop { get; }

    /// <summary>
    /// Whether request is from bot/crawler (search engine, monitoring).
    /// </summary>
    bool IsBot { get; }

    // ========================================
    // OPERATING SYSTEM
    // ========================================

    /// <summary>
    /// Client operating system family (Windows, macOS, iOS, Android, Linux, Unknown).
    /// </summary>
    OperatingSystemFamily OperatingSystem { get; }

    /// <summary>
    /// Operating system version (if detectable).
    /// Example: "Windows 11", "iOS 17.2", "Android 14"
    /// </summary>
    string? OperatingSystemVersion { get; }

    // ========================================
    // BROWSER
    // ========================================

    /// <summary>
    /// Browser family (Chrome, Firefox, Safari, Edge, Opera, Unknown).
    /// </summary>
    BrowserFamily Browser { get; }

    /// <summary>
    /// Browser version (if detectable).
    /// Example: "120.0.6099.109" (Chrome), "119.0" (Firefox)
    /// </summary>
    string? BrowserVersion { get; }

    /// <summary>
    /// Browser major version number.
    /// Example: 120 for Chrome 120.x.x
    /// </summary>
    int? BrowserMajorVersion { get; }

    /// <summary>
    /// Whether browser is evergreen (auto-updates).
    /// True for Chrome, Firefox, Edge, Safari (on latest OS).
    /// False for IE, legacy browsers.
    /// </summary>
    bool IsEvergreenBrowser { get; }

    // ========================================
    // CAPABILITIES
    // ========================================

    /// <summary>
    /// Device capabilities (features supported by device/browser).
    /// </summary>
    DeviceCapabilities Capabilities { get; }

    // ========================================
    // RAW DATA
    // ========================================

    /// <summary>
    /// Raw User-Agent string.
    /// </summary>
    string? UserAgent { get; }

    /// <summary>
    /// Device manufacturer (if detectable).
    /// Example: "Apple", "Samsung", "Google"
    /// </summary>
    string? Manufacturer { get; }

    /// <summary>
    /// Device model (if detectable).
    /// Example: "iPhone 15 Pro", "Pixel 8", "Galaxy S24"
    /// </summary>
    string? Model { get; }

    // ========================================
    // HELPERS
    // ========================================

    /// <summary>
    /// Get friendly device description.
    /// Example: "iPhone 15 Pro (iOS 17.2, Safari 17.2)"
    /// </summary>
    string GetDeviceDescription();
}

/// <summary>
/// Device type categories.
/// </summary>
public enum DeviceType
{
    Unknown = 0,
    Desktop = 1,
    Mobile = 2,
    Tablet = 3,
    Bot = 4,
    Console = 5, // Gaming console
    TV = 6,      // Smart TV
    Wearable = 7 // Smartwatch, etc.
}

/// <summary>
/// Operating system families.
/// </summary>
public enum OperatingSystemFamily
{
    Unknown = 0,
    Windows = 1,
    MacOS = 2,
    iOS = 3,
    Android = 4,
    Linux = 5,
    ChromeOS = 6
}

/// <summary>
/// Browser families.
/// </summary>
public enum BrowserFamily
{
    Unknown = 0,
    Chrome = 1,
    Firefox = 2,
    Safari = 3,
    Edge = 4,
    Opera = 5,
    InternetExplorer = 6,
    Samsung = 7, // Samsung Internet
    Brave = 8,
    Vivaldi = 9
}

/// <summary>
/// Device/browser capabilities.
/// </summary>
public record DeviceCapabilities
{
    /// <summary>
    /// Whether device supports touch input.
    /// </summary>
    public bool SupportsTouch { get; init; }

    /// <summary>
    /// Whether browser supports modern JavaScript (ES6+).
    /// </summary>
    public bool SupportsModernJavaScript { get; init; }

    /// <summary>
    /// Whether browser supports WebAssembly.
    /// </summary>
    public bool SupportsWebAssembly { get; init; }

    /// <summary>
    /// Whether browser supports Service Workers (PWA).
    /// </summary>
    public bool SupportsServiceWorkers { get; init; }

    /// <summary>
    /// Whether browser supports WebSockets.
    /// </summary>
    public bool SupportsWebSockets { get; init; }

    /// <summary>
    /// Whether browser supports modern CSS (Grid, Flexbox, Custom Properties).
    /// </summary>
    public bool SupportsModernCss { get; init; }

    /// <summary>
    /// Estimated screen width category (Small, Medium, Large).
    /// Based on device type heuristics.
    /// </summary>
    public ScreenSize EstimatedScreenSize { get; init; }

    /// <summary>
    /// Whether browser is considered secure/modern.
    /// False for IE11, outdated browsers with known vulnerabilities.
    /// </summary>
    public bool IsSecureBrowser { get; init; }
}

/// <summary>
/// Screen size categories.
/// </summary>
public enum ScreenSize
{
    Unknown = 0,
    Small = 1,   // Mobile phones
    Medium = 2,  // Tablets
    Large = 3    // Desktops/laptops
}
