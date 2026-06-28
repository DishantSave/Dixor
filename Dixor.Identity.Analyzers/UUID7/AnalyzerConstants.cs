namespace Dixor.Identity.Analyzers.UUID7;

internal static class AnalyzerConstants
{
    public const int MaxPrefixLength = 50;

    public const int MaxSuffixLength = 50;

    public static readonly HashSet<char> AllowedSeparators =
    [
        '-',
        ':',
        '~',
        '|',
        '{',
        '}',
        '[',
        ']',
        '<',
        '>',
        '/',
        '\\',
        '_'
    ];
}