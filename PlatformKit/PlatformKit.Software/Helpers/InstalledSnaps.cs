namespace PlatformKit.Software;

using PlatformKit;

public class InstalledSnaps
{
    /// <summary>
    /// Detect what Snap packages (if any) are installed on a linux distribution or on macOS.
    /// </summary>
    /// <returns>Returns a list of installed snaps. Returns an empty array if no Snaps are installed.</returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a Platform other than Linux, macOS, and FreeBsd.</exception>
    public static AppModel[] Get()
    {
        List<AppModel> apps = new List<AppModel>();

        if (PlatformAnalyzer.IsLinux() || PlatformAnalyzer.IsLinux() || PlatformAnalyzer.IsFreeBSD())
        {
            bool useSnap = Directory.Exists("/snap/bin");

            if (useSnap)
            {
                string[] snapResults = CommandRunner.RunCommandOnLinux("ls /snap/bin").Split(' ');

                foreach (var snap in snapResults)
                {
                    apps.Add(new AppModel(snap, "/snap/bin"));
                }

                return apps.ToArray();
            }

            apps.Clear();
            return apps.ToArray();
        }

        throw new PlatformNotSupportedException();
    }
}