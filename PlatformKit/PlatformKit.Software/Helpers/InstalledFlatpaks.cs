namespace PlatformKit.Software;

public class InstalledFlatpaks
{
    /// <summary>
    /// Platforms Supported On: Linux and FreeBsd.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static AppModel[] Get()
    {
        List<AppModel> apps = new List<AppModel>();

        if (PlatformAnalyzer.IsLinux() || PlatformAnalyzer.IsFreeBSD())
        {
            bool useFlatpaks;
            
            string[] flatpakTest = CommandRunner.RunCommandOnLinux("flatpak --version").Split(' ');
                
            if (flatpakTest[0].Contains("Flatpak"))
            {
                Version.Parse(flatpakTest[1]);

                useFlatpaks = true;
            }
            else
            {
                useFlatpaks = false;
            }

            if (useFlatpaks)
            {
#if NET5_0_OR_GREATER
                string[] flatpakResults = CommandRunner.RunCommandOnLinux("flatpak list --columns=name")
                    .Split(Environment.NewLine);
#else
                string[] flatpakResults = CommandRunner.RunCommandOnLinux("flatpak list --columns=name")
                    .Split(Convert.ToChar(Environment.NewLine));
#endif

                var installLocation = CommandRunner.RunCommandOnLinux("flatpak --installations");
                
                foreach (var flatpak in flatpakResults)
                {
                    apps.Add(new AppModel(flatpak, installLocation));
                }
                
                return apps.ToArray();
            }

            apps.Clear();
            return apps.ToArray();
        }

        throw new PlatformNotSupportedException();
    }
}