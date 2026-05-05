namespace BidirectionalDictionary.Benchmarks.Columns;

internal static class MethodNameParser
{
    private const string DefaultSuffix = "_Default";
    private const string BidirectionalSuffix = "_Bidirectional";

    public static (string Operation, string Type) Parse(string methodName)
    {
        if (methodName.EndsWith(DefaultSuffix, StringComparison.Ordinal))
        {
            return (methodName[..^DefaultSuffix.Length], "Default");
        }

        if (methodName.EndsWith(BidirectionalSuffix, StringComparison.Ordinal))
        {
            return (methodName[..^BidirectionalSuffix.Length], "Bidirectional");
        }

        return (methodName, "?");
    }
}
