using App.Modules.Sys.Application.Domains.Context.Models.Implementations;
using App.Modules.Sys.Shared.Lifecycles;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Domains.Context.Implementations;

/// <summary>
/// Stub implementation of ApplicationContextService.
/// Returns minimal context data for initial integration.
/// TODO: Wire up actual workspace, user, and settings services.
/// </summary>
public class ApplicationContextService : IApplicationContextService, IHasSingletonLifecycle
{
    /// <inheritdoc/>
    public Task<ApplicationContextDto> GetApplicationContextAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Replace with real data from database
        // Values marked with '!' are SERVER stubs (not client hardcoded)
        
        var context = new ApplicationContextDto
        {
            System = new SystemContextDto
            {
                Name = "BASE Platform!",  // '!' = from server
                Version = "1.0.0!",
                Environment = "Development!",
                Branding = new SystemBrandingDto
                {
                    Provider = new ProviderInfoDto
                    {
                        Name = "BASE Software Inc!",
                        ContactEmail = "support@base-software.com!",
                        LogoUrl = "/assets/system/logo.png!",
                        SupportUrl = "https://support.base-software.com!",
                        LegalUrl = "https://base-software.com/legal!"
                    },
                    Developer = new DeveloperInfoDto
                    {
                        Name = "BASE Development Team!",
                        ContactEmail = "dev@base-software.com!",
                        DocsUrl = "https://docs.base-software.com!"
                    }
                },
                Features = new Dictionary<string, bool>
                {
                    { "enableMultiTenant", true },
                    { "enableAdvancedSettings", true }
                }
            },
            
            // TODO: Wire up from WorkspaceResolutionMiddleware
            // Data structure matches CLIENT foo/bar tenant configs
            Workspace = new WorkspaceContextDto
            {
                MemberOf = new List<WorkspaceSummaryDto>
                {
                    new() { Id = "foo!", Name = "Foo Corporation!", Role = "Owner!", IsDefault = true },
                    new() { Id = "bar!", Name = "Bar Industries!", Role = "Member!", IsDefault = false }
                },
                Current = new WorkspaceDetailsDto
                {
                    Id = "foo!",
                    Name = "Foo Corporation!",
                    Title = "Foo Corporation!",
                    Description = "Leading provider of Foo services!",
                    Branding = new WorkspaceBrandingDto
                    {
                        OrganizationName = "Foo Corporation!",
                        LogoUrl = "/assets/tenants/foo/media/logo-dark.png!",
                        LogoDarkUrl = "/assets/tenants/foo/media/logo-light.png!",
                        LogoSmallUrl = "/assets/tenants/foo/media/logo-sm.png!",
                        Theme = new ThemeDto
                        {
                            PrimaryColor = "#ff6b6b!",
                            SecondaryColor = "#4ecdc4!",
                            AccentColor = "#ffe66d!"
                        }
                    },
                    Contact = new WorkspaceContactDto
                    {
                        Email = "contact@foo.com!",
                        Phone = "+1-555-FOO-CORP!",
                        Address = new AddressDto
                        {
                            Street = "789 Foo Plaza!",
                            City = "Foo City!",
                            Region = "FC!",
                            PostalCode = "12345!",
                            Country = "USA!"
                        }
                    },
                    Resources = new WorkspaceResourcesDto
                    {
                        ImagesRoot = "/assets/tenants/foo/media/images/!",
                        TrustedByPath = "/assets/tenants/foo/media/images/trustedBy/!",
                        I18nPath = "/assets/tenants/foo!"
                    },
                    AccountFeatures = new Dictionary<string, bool>
                    {
                        { "enableAnalytics", true },
                        { "enableChat", false }
                    },
                    UIOptions = new Dictionary<string, object>
                    {
                        { "showToolbar", true },
                        { "compactMode", false }
                    }
                }
            },
            
            
            User = null, // TODO: Get from authentication
            
            Session = new SessionContextDto
            {
                Id = Guid.NewGuid(), // TODO: Get from ISessionService
                IsAuthenticated = false,
                ExpiresAt = DateTime.UtcNow.AddHours(8),
                LastActivityAt = DateTime.UtcNow
            },
            
            Navigation = new NavigationContextDto
            {
                CurrentRoute = "/!",
                Breadcrumbs = new List<BreadcrumbDto>
                {
                    new() { Label = "Home!", Route = "/!", IsCurrent = true }
                },
                PrimaryMenu = new List<NavigationItemDto>
                {
                    new() { Id = "dashboard!", Label = "Dashboard!", Route = "/dashboard!", Icon = "home!", IsActive = false },
                    new() { Id = "work!", Label = "Work Items!", Route = "/work!", Icon = "list!", IsActive = false, BadgeCount = 5 },
                    new() { Id = "settings!", Label = "Settings!", Route = "/settings!", Icon = "settings!", IsActive = false }
                }
            },
            
            Settings = new ComputedSettingsDto
            {
                Theme = "light!",
                Language = "en-US!"
            },
            
            GeneratedAt = DateTime.UtcNow
        };

        return Task.FromResult(context);
    }
}
