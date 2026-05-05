using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BidirectionalDictionary.Benchmarks.Columns;

internal sealed class OperationColumn : MethodNamePartColumn
{
    public override string Id => nameof(OperationColumn);
    public override string ColumnName => "Operation";
    public override string Legend => "The operation under test";
    public override int PriorityInCategory => -100;

    public override string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        return MethodNameParser
            .Parse(benchmarkCase.Descriptor.WorkloadMethod.Name)
            .Operation;
    }
}
