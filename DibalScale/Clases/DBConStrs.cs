using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
namespace DibalScale.Clases
{
    class DBConStrs
    {
        public static string ConnectionString()
        {
            try
            {
                ConfigTxt config = new ConfigTxt();
                return "Data Source=" + config.Server + @"\" + config.SqlServer + ";Initial Catalog=" + config.DateBase + ";Integrated Security=True";
            }
            catch(Exception ex)
            {
                Log.Write(ex, " - Ошибка в class DBConStrs");
                throw ex;
            }
        }

    }
   
}
