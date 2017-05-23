using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisenessLogicLayer
{
    public static class ServerHelper
    {
        public static bool WriteError(Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.InnerException?.Message ?? ex.Message);
            return false;
        }
    }
}
