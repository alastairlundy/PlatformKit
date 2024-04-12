using System;
using System.Text;

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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    /// <param name="str"></param>
    /// <param name="numberOfZeroesNeeded">The number of zeroes to add. Valid values are 0 through 3. Defaults to 3.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    internal static string AddMissingZeroes(this string str, int numberOfZeroesNeeded = 3)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(str);
        
        int dots = str.CountDotsInString();

        if (dots == 0)
        {
            stringBuilder.Append('.');
            stringBuilder.Append('0');
        }
        if (dots == 1 && numberOfZeroesNeeded > 1)
        {
            stringBuilder.Append('.');
            stringBuilder.Append('0');
        }
        if (dots == 2 && numberOfZeroesNeeded > 2)
        {
            stringBuilder.Append('.');
            stringBuilder.Append('0');
        }
        if (dots == 3 && numberOfZeroesNeeded == 3)
        {
            return str;
        }

        int newDots = stringBuilder.ToString().CountDotsInString();
        
        if (newDots == 1 && numberOfZeroesNeeded == 1 || 
            newDots == 2 && numberOfZeroesNeeded == 2 || 
            newDots == 3 && numberOfZeroesNeeded == 3)
        {
            return stringBuilder.ToString();
        }
        
        throw new ArgumentException();
    }
}