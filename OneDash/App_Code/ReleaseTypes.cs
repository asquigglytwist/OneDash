/// <summary>
/// Represents different types of releases.
/// </summary>
public enum ReleaseTypes
{
    /// <summary>
    /// A Major (version) release.  Sometimes products treat a minor version release as a Major release, considering the scope of change.
    /// </summary>
    Major,
    /// <summary>
    /// RePost of an already released version - typically to rectify issues immediately in Production.
    /// </summary>
    RePost,
    /// <summary>
    /// Patch release for a Major version.
    /// </summary>
    Patch,
    /// <summary>
    /// As the name implies, a HotFix release.
    /// </summary>
    HotFix
}