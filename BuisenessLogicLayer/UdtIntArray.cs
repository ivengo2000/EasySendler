using EntityFrameworkExtras.EF6;

namespace BuisenessLogicLayer
{
    [UserDefinedTableType("UdtIntArray")]
    public class UdtIntArray
    {
        [UserDefinedTableTypeColumn(1)]
        public int Value { get; set; }
    }
}
