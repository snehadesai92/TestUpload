using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace QUERION.Core.DBClass
{
    public static class ApiUtility
    {
        public static string ConnectionString =
            ConfigurationManager.ConnectionStrings["QuerionConnectionString"].ConnectionString;
    }
}
