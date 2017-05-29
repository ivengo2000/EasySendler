using System.Collections.Generic;
using System.Data;
using EntityFrameworkExtras.EF6;

namespace BuisenessLogicLayer
{
    [StoredProcedure("sp_SaveConfiguredRecipientList")]
    public class SpSaveConfiguredRecipientList
    {
        [StoredProcedureParameter(SqlDbType.Int, ParameterName = "listId")]
        public int ListId { get; set; }

        [StoredProcedureParameter(SqlDbType.Udt, ParameterName = "ids")]
        public List<UdtIntArray> Ids { get; set; }
    }
}
