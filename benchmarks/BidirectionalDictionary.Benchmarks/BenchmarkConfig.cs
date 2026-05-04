using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using Benchmarks.Columns;

namespace Benchmarks;

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
