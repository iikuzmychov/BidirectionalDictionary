using BenchmarkDotNet.Columns;

namespace BidirectionalDictionary.Benchmarks.Columns;

internal sealed class HideBuiltInColumnsRule : IColumnHidingRule
{
    public bool NeedToHide(IColumn column)
    {
        if (column is OperationColumn or TypeColumn)
        {
            return false;
        }

        if (column.Category is not ColumnCategory.Job)
        {
            return false;
        }

        return true;
    }
}
