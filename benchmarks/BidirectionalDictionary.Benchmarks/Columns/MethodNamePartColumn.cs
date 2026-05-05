using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BidirectionalDictionary.Benchmarks.Columns;

internal abstract class MethodNamePartColumn : IColumn
{
    public abstract string Id { get; }
    public abstract string ColumnName { get; }
    public abstract string Legend { get; }
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Job;
    public abstract int PriorityInCategory { get; }
    public bool IsNumeric => false;
    public UnitType UnitType => UnitType.Dimensionless;

    public bool IsAvailable(Summary summary) => true;
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style)
    {
        return GetValue(summary, benchmarkCase);
    }

    public abstract string GetValue(Summary summary, BenchmarkCase benchmarkCase);

    public override string ToString() => ColumnName;
}
