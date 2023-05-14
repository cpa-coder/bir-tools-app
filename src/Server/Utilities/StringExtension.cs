using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace System;

public static class StringExtension
{
    /// <summary>
    ///     Validate Tax Identification Number based on Philippine format.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsValidTin(this string str)
    {
        try
        {
            const int minLength = 11;
            var format = str.Length <= minLength
                ? new Regex(@"^[0-9]\d{2}-[0-9]\d{2}-[0-9]\d{2}$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(1))
                : new Regex(@"^[0-9]\d{2}-[0-9]\d{2}-[0-9]\d{2}-[0-9]\d{2,4}$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(1));

            return format.IsMatch(str);
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}