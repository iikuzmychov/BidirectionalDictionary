using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BidirectionalDictionary.Benchmarks.Columns;

namespace BidirectionalDictionary.Benchmarks;

public sealed class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddDiagnoser(MemoryDiagnoser.Default);

        AddColumn(new OperationColumn());
        AddColumn(new TypeColumn());

        HideColumns(new HideBuiltInColumnsRule());
    }
}
