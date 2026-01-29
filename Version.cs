namespace SerenityStarMcp;

/// <summary>
/// Application version information following Semantic Versioning 2.0.0
/// </summary>
public static class Version
{
    /// <summary>
    /// Major version number (breaking changes)
    /// </summary>
    public const int Major = 1;
    
    /// <summary>
    /// Minor version number (new features, backward compatible)
    /// </summary>
    public const int Minor = 0;
    
    /// <summary>
    /// Patch version number (bug fixes, backward compatible)
    /// </summary>
    public const int Patch = 4;
    
    /// <summary>
    /// Full semantic version string
    /// </summary>
    public static string FullVersion => $"{Major}.{Minor}.{Patch}";
    
    /// <summary>
    /// Build timestamp (UTC)
    /// </summary>
    public static readonly DateTime BuildTime = DateTime.UtcNow;
}