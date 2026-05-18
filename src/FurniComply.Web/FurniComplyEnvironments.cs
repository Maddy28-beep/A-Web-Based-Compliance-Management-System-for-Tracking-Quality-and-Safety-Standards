namespace FurniComply.Web;

/// <summary>
/// Local hosting profiles used for capstone defense and developer workstations (not public production).
/// </summary>
internal static class FurniComplyEnvironments
{
    public const string Capstone = "Capstone";

    public static bool IsCapstoneOrDevelopment(IHostEnvironment environment) =>
        environment.IsDevelopment() ||
        environment.IsEnvironment(Capstone);
}
