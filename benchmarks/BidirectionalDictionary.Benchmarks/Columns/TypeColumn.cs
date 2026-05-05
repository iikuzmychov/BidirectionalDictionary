using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BidirectionalDictionary.Benchmarks.Columns;

internal sealed class TypeColumn : MethodNamePartColumn
{
    public override string Id => nameof(TypeColumn);
    public override string ColumnName => "Type";
    public override string Legend => "Default = BCL Dictionary/ReadOnlyDictionary; Bidirectional = library variant";
    public override int PriorityInCategory => -99;

    public override string GetValue(Summary summary, BenchmarkCase benchmarkCase)
        => MethodNameParser.Parse(benchmarkCase.Descriptor.WorkloadMethod.Name).Type;
}
