using DotNetEnv;

namespace FurniComply.Web;

internal static class DotEnvBootstrap
{
    internal static void LoadIfPresent()
    {
        foreach (var path in EnumerateCandidates())
        {
            if (!File.Exists(path))
                continue;

            Env.Load(path);
            return;
        }
    }

    private static IEnumerable<string> EnumerateCandidates()
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var webProjectEnv = ResolveWebProjectPath();
        if (webProjectEnv != null && seen.Add(webProjectEnv))
            yield return webProjectEnv;

        foreach (var root in GetSearchRoots())
        {
            string path;
            try
            {
                path = Path.GetFullPath(Path.Combine(root, ".env"));
            }
            catch
            {
                continue;
            }

            if (seen.Add(path))
                yield return path;
        }
    }

    private static List<string> GetSearchRoots()
    {
        var roots = new List<string>();
        try
        {
            roots.Add(Directory.GetCurrentDirectory());
        }
        catch
        {
        }

        try
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            for (var i = 0; i < 10 && dir != null; i++)
            {
                roots.Add(dir.FullName);
                dir = dir.Parent;
            }
        }
        catch
        {
        }

        return roots;
    }

    private static string? ResolveWebProjectPath()
    {
        try
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "FurniComply.Web.csproj")))
                    return Path.Combine(dir.FullName, ".env");
                dir = dir.Parent;
            }
        }
        catch
        {
        }

        return null;
    }
}
