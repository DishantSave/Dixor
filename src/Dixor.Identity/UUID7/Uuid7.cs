using Dixor.Identity.UUID7.Internal;

namespace Dixor.Identity.UUID7;

public static class Uuid7
{
    public static Guid NewGuid()
        => Uuid7Generator.Generate();

    public static Guid NewGuid(DateTimeOffset timestamp)
        => Uuid7Generator.Generate(timestamp);

    public static Guid NewMonotonicGuid()
        => Uuid7MonotonicGenerator.Generate();

    public static IEnumerable<Guid> NewGuids(int count)
        => Uuid7BatchGenerator.Generate(count);

    public static DateTimeOffset GetTimestamp(Guid guid)
        => Uuid7TimestampExtractor.Extract(guid);

    public static bool TryGetTimestamp(
        Guid guid,
        out DateTimeOffset timestamp)
        => Uuid7TimestampExtractor.TryExtract(
            guid,
            out timestamp);

    public static long GetUnixTimeMilliseconds(Guid guid)
        => Uuid7TimestampExtractor
            .Extract(guid)
            .ToUnixTimeMilliseconds();

    public static bool IsUuid7(Guid guid)
        => Uuid7Validator.IsUuid7(guid);

    public static bool IsValid(string input)
        => Uuid7Validator.IsValid(input);

    public static string Format(
        Guid guid,
        string? prefix = null,
        string? suffix = null,
        string separator = "-")
        => Uuid7Formatter.Format(
            guid,
            prefix,
            suffix,
            separator);

    public static string NewString(
        string? prefix = null,
        string? suffix = null,
        string separator = "-")
        => Uuid7Formatter.Format(
            NewGuid(),
            prefix,
            suffix,
            separator);

    public static Guid Parse(string input)
        => Uuid7Parser.Parse(input);

    public static bool TryParse(
        string input,
        out Guid guid)
        => Uuid7Parser.TryParse(input, out guid);
}