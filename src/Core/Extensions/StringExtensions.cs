namespace TemplateFramework.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Replaces line endings to the right format conform the current operating system
    /// </summary>
    /// <param name="instance">String instance to get the normalized line endings for</param>
    /// <param name="matchTimeoutInMilliseconds">Time-out in milli secconds for the regular expression parsing</param>
    /// <returns></returns>
    public static string NormalizeLineEndings(this string instance, int matchTimeoutInMilliseconds = 500)
        => Regex.Replace(instance, @"\r\n|\n\r|\n|\r", Environment.NewLine, RegexOptions.None, TimeSpan.FromMilliseconds(matchTimeoutInMilliseconds));
}
