using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Emby.Naming.Common;

namespace Emby.Naming.Video.DateTimeResolvers;

/// <summary>
/// If a timestamp is explititly defined by brackets () or []
/// Eg. "1883[2021]" will use 2021 as the date.
/// </summary>
public class TimeExplicitlyInBracketsMovieDateTimeResolver : IMovieDateTimeResolver
{
    /// <summary>
    /// Attempts to resolve date and Name from the provided fileName.
    /// </summary>
    /// <param name="fileName">Name of video.</param>
    /// <param name="namingOptions">NamingOptions.</param>
    /// <returns>null if could not resolve.</returns>
    public CleanDateTimeResult? Resolve(string fileName, NamingOptions namingOptions)
    {
        var regex = new Regex(@"((\[|\()(?'date'\d{4})(\]|\)))");

        var match = regex.Match(fileName);

        if (!match.Success)
        {
            return null;
        }

        match.Groups.TryGetValue("date", out var dateGroup);

        if (dateGroup?.Value == null)
        {
            return null;
        }

        var name = DateTimeResolverHelpers.GetBestNameMatchAfterRemovalOfDate(fileName, match.Value, namingOptions);

        return new CleanDateTimeResult(name, dateGroup.Value);
    }
}
