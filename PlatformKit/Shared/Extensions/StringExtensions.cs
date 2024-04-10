namespace PlatformKit.Shared.Extensions;

internal static class StringExtensions
{
    internal static int CountDotsInString(this string str)
    {
        int dotCounter = 0;

        foreach (char c in str)
        {
            if (c == '.')
            {
                dotCounter++;
            }
        }

        return dotCounter;
    }
}