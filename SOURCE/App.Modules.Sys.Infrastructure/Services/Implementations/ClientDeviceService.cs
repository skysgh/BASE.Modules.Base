using App.Modules.Sys.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System;

namespace App.Modules.Sys.Infrastructure.Services.Implementations;

/// <summary>
/// Production implementation of client device service.
/// Extracts device information from HTTP request headers.
/// </summary>
/// <remarks>
/// Scoped lifetime - per HTTP request.
/// Parses User-Agent and other headers to identify client device.
/// </remarks>
public sealed class ClientDeviceService : IClientDeviceService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClientDeviceService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    private HttpContext? HttpContext => _httpContextAccessor.HttpContext;

    public string UserAgent
    {
        get
        {
            var userAgent = HttpContext?.Request.Headers["User-Agent"].ToString();
            return string.IsNullOrEmpty(userAgent) ? "Unknown" : userAgent;
        }
    }

    public string IpAddress
    {
        get
        {
            if (HttpContext == null)
            {
                return "Unknown";
            }

            // Check for X-Forwarded-For (proxy/load balancer)
            var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                // Take first IP if multiple
                var ips = forwardedFor.Split(',');
                return ips[0].Trim();
            }

            // Check for X-Real-IP (nginx)
            var realIp = HttpContext.Request.Headers["X-Real-IP"].ToString();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            // Fallback to connection remote IP
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }

    public string? AcceptLanguage
    {
        get
        {
            var acceptLanguage = HttpContext?.Request.Headers["Accept-Language"].ToString();
            return string.IsNullOrEmpty(acceptLanguage) ? null : acceptLanguage;
        }
    }

    public string? AcceptEncoding
    {
        get
        {
            var acceptEncoding = HttpContext?.Request.Headers["Accept-Encoding"].ToString();
            return string.IsNullOrEmpty(acceptEncoding) ? null : acceptEncoding;
        }
    }

    public bool IsMobile
    {
        get
        {
            var userAgent = UserAgent.ToLowerInvariant();
            return userAgent.Contains("mobile") ||
                   userAgent.Contains("android") ||
                   userAgent.Contains("iphone") ||
                   userAgent.Contains("ipad") ||
                   userAgent.Contains("ipod") ||
                   userAgent.Contains("blackberry") ||
                   userAgent.Contains("windows phone");
        }
    }

    public bool IsTablet
    {
        get
        {
            var userAgent = UserAgent.ToLowerInvariant();
            return (userAgent.Contains("tablet") ||
                    userAgent.Contains("ipad")) &&
                   !userAgent.Contains("mobile");
        }
    }

    public bool IsDesktop => !IsMobile && !IsTablet;

    public bool IsBot
    {
        get
        {
            var userAgent = UserAgent.ToLowerInvariant();
            return userAgent.Contains("bot") ||
                   userAgent.Contains("crawler") ||
                   userAgent.Contains("spider") ||
                   userAgent.Contains("scraper");
        }
    }

    public string GetBrowserName()
    {
        var userAgent = UserAgent.ToLowerInvariant();

        if (userAgent.Contains("edg/")) return "Edge";
        if (userAgent.Contains("chrome/")) return "Chrome";
        if (userAgent.Contains("firefox/")) return "Firefox";
        if (userAgent.Contains("safari/") && !userAgent.Contains("chrome")) return "Safari";
        if (userAgent.Contains("opera/") || userAgent.Contains("opr/")) return "Opera";
        if (userAgent.Contains("msie") || userAgent.Contains("trident/")) return "Internet Explorer";

        return "Unknown";
    }

    public string GetOperatingSystem()
    {
        var userAgent = UserAgent.ToLowerInvariant();

        if (userAgent.Contains("windows nt 10.0")) return "Windows 10/11";
        if (userAgent.Contains("windows nt 6.3")) return "Windows 8.1";
        if (userAgent.Contains("windows nt 6.2")) return "Windows 8";
        if (userAgent.Contains("windows nt 6.1")) return "Windows 7";
        if (userAgent.Contains("windows")) return "Windows";

        if (userAgent.Contains("mac os x")) return "macOS";
        if (userAgent.Contains("iphone")) return "iOS (iPhone)";
        if (userAgent.Contains("ipad")) return "iOS (iPad)";
        if (userAgent.Contains("android")) return "Android";
        if (userAgent.Contains("linux")) return "Linux";

        return "Unknown";
    }

    public string? GetDeviceType()
    {
        if (IsBot) return "Bot";
        if (IsTablet) return "Tablet";
        if (IsMobile) return "Mobile";
        if (IsDesktop) return "Desktop";
        return null;
    }

    public int? GetScreenWidth()
    {
        // Would need JavaScript to report actual screen dimensions
        // Return null as we can't reliably determine from server-side
        return null;
    }

    public int? GetScreenHeight()
    {
        // Would need JavaScript to report actual screen dimensions
        return null;
    }
}
