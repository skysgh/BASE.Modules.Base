# API Documentation Architecture

## Overview

This project provides **module-based, versioned API documentation** using:
- **OpenAPI** (native .NET 10) - Machine-readable JSON ✅ WORKING
- **Swagger UI** - Traditional interactive documentation ✅ WORKING
- **Scalar UI** - Modern API explorer ⚠️ DISABLED (pending investigation)

## ⚠️ Scalar Status

**DISABLED** in Program.cs due to auto-discovery issue:
- Scalar looks for document named "v1"
- We have "sys-v1" (module-based naming)
- Investigation needed on Scalar's document discovery mechanism
- Infrastructure remains in place for future use

**Current Working UIs:**
- ✅ Swagger UI at `/documentation/apis/swagger/sys/v1/`
- ❌ Scalar UI disabled (see issue above)

---

### ✅ **MODULE-BASED**
Each **Logical Module** gets its own isolated documentation to prevent overwhelming users:
- **System (Sys)** - Core system management
- **Social** (future) - Social features
- **Work** (future) - Work management
- etc.

### ✅ **VERSIONED**
Every endpoint includes explicit version information:
- Prevents breaking changes
- Supports multiple API versions simultaneously
- Clear upgrade paths for clients

### ✅ **STANDARDS-COMPLIANT**
Machine-readable JSON at **industry-standard paths** that tools expect:
- `/openapi/{module}-{version}.json` - Native .NET OpenAPI
- `/swagger/{module}-{version}/swagger.json` - Swashbuckle OpenAPI

Human-readable UIs at **custom organized paths**:
- `/documentation/apis/{module}/{version}/swagger` - Swagger UI
- `/documentation/apis/{module}/{version}/scalar` - Scalar UI

---

## URL Structure

### Current (System Module)

| Type | URL | Purpose |
|---|---|---|
| **OpenAPI JSON** | `/openapi/sys-v1.json` | Tools/generators |
| **Swagger JSON** | `/swagger/sys-v1/swagger.json` | Tools/generators |
| **Swagger UI** | `/documentation/apis/sys/v1/swagger` | Human browsing |
| **Scalar UI** | `/documentation/apis/sys/v1/scalar` | Human browsing |

### Future (Social Module Example)

| Type | URL |
|---|---|
| **OpenAPI JSON** | `/openapi/social-v1.json` |
| **Swagger JSON** | `/swagger/social-v1/swagger.json` |
| **Swagger UI** | `/documentation/apis/social/v1/swagger` |
| **Scalar UI** | `/documentation/apis/social/v1/scalar` |

---

## Adding a New Module

### Step 1: Register Documentation in `Program.cs`

```csharp
// In builder.Services section:
builder.Services.AddApiDocumentation(moduleName: "sys", apiVersion: "v1");
builder.Services.AddApiDocumentation(moduleName: "social", apiVersion: "v1");  // NEW MODULE
builder.Services.AddApiDocumentation(moduleName: "work", apiVersion: "v1");    // NEW MODULE

// In app.Use section:
app.UseApiDocumentation(moduleName: "sys", apiVersion: "v1");
app.UseApiDocumentation(moduleName: "social", apiVersion: "v1");  // NEW MODULE
app.UseApiDocumentation(moduleName: "work", apiVersion: "v1");    // NEW MODULE
```

### Step 2: Ensure Controllers Follow Naming Convention

**CRITICAL:** Controllers MUST be in a namespace that includes the module name:

```csharp
// ✅ CORRECT - Will appear in "social" module docs
namespace App.Modules.Social.Interfaces.API.REST.Controllers.V1
{
    [ApiController]
    [Route("api/v1/social/friends")]
    public class FriendsController : ControllerBase
    {
        // Your endpoints here
    }
}

// ❌ WRONG - Won't be filtered correctly
namespace App.Controllers
{
    // ...
}
```

**Namespace Pattern:** `App.Modules.{ModuleName}.{Rest}.{Of}.{Namespace}`

The documentation system uses this pattern to automatically filter endpoints:
- `App.Modules.Sys.*` → System module docs
- `App.Modules.Social.*` → Social module docs
- `App.Modules.Work.*` → Work module docs

### Step 3: Test Your New Module

1. **Run the app** (F5)
2. **Navigate to your module's Swagger UI:**
   - System: `https://localhost:4204/documentation/apis/sys/v1/swagger`
   - Social: `https://localhost:4204/documentation/apis/social/v1/swagger`
   - Work: `https://localhost:4204/documentation/apis/work/v1/swagger`
3. **Verify endpoints appear** - Only endpoints from that module's namespace should show
4. **Test Scalar UI** - Same URLs but replace `/swagger` with `/scalar`
5. **Verify JSON endpoints** work for tools:
   - OpenAPI: `/openapi/social-v1.json`
   - Swagger: `/swagger/social-v1/swagger.json`

---

## Version Upgrades

When releasing a **breaking change**, increment the version:

```csharp
// Old version (keep for backward compatibility)
builder.Services.AddApiDocumentation(moduleName: "sys", apiVersion: "v1");
app.UseApiDocumentation(moduleName: "sys", apiVersion: "v1");

// New version (breaking changes)
builder.Services.AddApiDocumentation(moduleName: "sys", apiVersion: "v2");
app.UseApiDocumentation(moduleName: "sys", apiVersion: "v2");
```

Both versions run **simultaneously**, allowing gradual client migration!

---

## Configuration Options

### Enable/Disable Specific UIs

```csharp
app.UseApiDocumentation(
    moduleName: "sys",
    apiVersion: "v1",
    enableOpenApi: true,   // Native .NET OpenAPI JSON
    enableSwagger: true,   // Swagger UI
    enableScalar: false);  // Disable Scalar if not needed
```

### XML Documentation

Add XML comments to your controllers/endpoints. They'll automatically appear in all three UIs!

```csharp
/// <summary>
/// Get all friends for the current user.
/// </summary>
/// <returns>List of friends with their profiles.</returns>
[HttpGet]
public async Task<ActionResult<IEnumerable<FriendDto>>> GetFriends()
{
    // Implementation
}
```

---

## Troubleshooting

### "My endpoints don't appear in the docs!"

**Cause:** Namespace filtering.

**Fix:** Ensure your controller namespace contains `.Modules.{YourModuleName}`:
```csharp
namespace App.Modules.Social.Interfaces.API.REST.Controllers.V1  // ✅ Contains ".Modules.Social"
```

### "Swagger dropdown shows wrong module name"

**Cause:** Module name casing must match namespace.

**Fix:** Module name is case-insensitive, but should match:
```csharp
// If namespace is: App.Modules.Social
builder.Services.AddApiDocumentation(moduleName: "social", ...)  // ✅ Lowercase is fine
```

The code capitalizes it automatically: "social" → "Social Module API"

### "Tools can't find my OpenAPI spec"

**Cause:** Using custom path instead of standard.

**Fix:** Tools look at these STANDARD paths (automatically configured):
- `/openapi/{module}-{version}.json`
- `/swagger/{module}-{version}/swagger.json`

Don't change these! Human UIs use custom paths (`/documentation/...`), but JSON stays at standards.

---

## Best Practices

### ✅ DO:
- Add XML comments to all public endpoints
- Use explicit versioning (never omit `apiVersion`)
- Keep module names lowercase and simple
- Follow namespace conventions strictly
- Test both JSON endpoints and UIs

### ❌ DON'T:
- Move JSON endpoints from standard paths
- Mix controllers from different modules in same namespace
- Skip version numbers (always include `v1`, `v2`, etc.)
- Forget to add both `AddApiDocumentation()` AND `UseApiDocumentation()`

---

## Package Dependencies

Required packages (already installed):
- `Microsoft.AspNetCore.OpenApi` (10.0.2+) - Native OpenAPI
- `Swashbuckle.AspNetCore` (10.1.1+) - Swagger UI/JSON
- `Scalar.AspNetCore` (2.12.32+) - Scalar UI
- `Microsoft.AspNetCore.Mvc.Versioning` (5.1.0+) - API versioning

---

## Questions?

See: `ApiDocumentationExtensions.cs` for implementation details.

**Architecture Owner:** System Module Team
**Last Updated:** 2026-02-04
