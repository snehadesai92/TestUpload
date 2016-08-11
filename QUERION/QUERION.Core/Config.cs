using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;


namespace QUERION.Core
{
    public class Config
    {/// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the default connection String as specified in the provider.
        /// </summary>
        /// <returns>The connection String</returns>
        /// <remarks></remarks>
        /// -----------------------------------------------------------------------------
        public static string GetConnectionString()
        {
            return GetConnectionString("");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets the specified connection String
        /// </summary>
        /// <param name="name">Name of Connection String to return</param>
        /// <returns>The connection String</returns>
        /// <remarks></remarks>
        public static string GetConnectionString(string name)
        {
            string connectionString = "";
            //First check if connection string is specified in <connectionstrings> (ASP.NET 2.0 / DNN v4.x)
            ////comment on 21042014
            ////if (!String.IsNullOrEmpty(name))
            ////{
            ////    DynamicEncrypt objEncr = new DynamicEncrypt();
            ////    string Data_Source =  Convert.ToString(WebConfigurationManager.AppSettings["Data_Source"]);
            ////    string Initial_Catalog = Convert.ToString(WebConfigurationManager.AppSettings["Initial_Catalog"]);
            ////    string uid = Convert.ToString(WebConfigurationManager.AppSettings["uid"]);
            ////    string password = Encryptor.DecryptString(Convert.ToString(WebConfigurationManager.AppSettings["password"]));
            ////    //string password = objEncr.Decrypt(Convert.ToString(WebConfigurationManager.AppSettings["password"]));

            ////   // string rt = Convert.ToString(WebConfigurationManager.AppSettings["password"]);
            ////   //string  assword = password.Replace(" ","");

            ////    connectionString = "Data Source=" + Data_Source + ";Initial Catalog=" + Initial_Catalog + ";uid=" + uid + ";password=" + password;
            ////    //@"Data Source=180.149.240.49;Initial Catalog=JMJGreen; uid=jmj_dbUser; password=jmj123!@#; ";

            ////    //ASP.NET 2 version connection string (in <connectionstrings>)
            ////    //This will be for new v4.x installs or upgrades from v4.x
            ////  //  connectionString = WebConfigurationManager.ConnectionStrings[name].ConnectionString;
            ////}


            //First check if connection string is specified in <connectionstrings> (ASP.NET 2.0 / DNN v4.x)
            if (!String.IsNullOrEmpty(name))
            {
                //ASP.NET 2 version connection string (in <connectionstrings>)
                //This will be for new v4.x installs or upgrades from v4.x
                connectionString = WebConfigurationManager.ConnectionStrings[name].ConnectionString;
            }

            if (String.IsNullOrEmpty(connectionString))
            {
                if (!String.IsNullOrEmpty(name))
                {
                    //Next check if connection string is specified in <appsettings> (ASP.NET 1.1 / DNN v3.x)
                    //This will accomodate upgrades from v3.x
                    connectionString = GetSetting(name);
                }
            }
            return connectionString;
        }
        //public static string GetConnectionString(string name)
        //{
        //    string connectionString = "";
        //    //First check if connection string is specified in <connectionstrings> (ASP.NET 2.0 / DNN v4.x)
        //    if (!String.IsNullOrEmpty(name))
        //    {
        //        DynamicEncrypt objEncr = new DynamicEncrypt();
        //        string Data_Source =  Convert.ToString(WebConfigurationManager.AppSettings["Data_Source"]);
        //        string Initial_Catalog = Convert.ToString(WebConfigurationManager.AppSettings["Initial_Catalog"]);
        //        string uid = Convert.ToString(WebConfigurationManager.AppSettings["uid"]);
        //        string password = Encryptor.DecryptString(Convert.ToString(WebConfigurationManager.AppSettings["password"]));
        //        //string password = objEncr.Decrypt(Convert.ToString(WebConfigurationManager.AppSettings["password"]));

        //       // string rt = Convert.ToString(WebConfigurationManager.AppSettings["password"]);
        //       //string  assword = password.Replace(" ","");

        //        connectionString = "Data Source=" + Data_Source + ";Initial Catalog=" + Initial_Catalog + ";uid=" + uid + ";password=" + password;
        //     //   @"Data Source=.\SQLEXPRESS;Initial Catalog=Reliance;Integrated Security = true;";

        //        //ASP.NET 2 version connection string (in <connectionstrings>)
        //        //This will be for new v4.x installs or upgrades from v4.x
        //      //  connectionString = WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        //    }
        //    if (String.IsNullOrEmpty(connectionString))
        //    {
        //        if (!String.IsNullOrEmpty(name))
        //        {
        //            //Next check if connection string is specified in <appsettings> (ASP.NET 1.1 / DNN v3.x)
        //            //This will accomodate upgrades from v3.x
        //            connectionString = GetSetting(name);
        //        }
        //    }
        //    return connectionString;  
        //}

        public static string GetSetting(string setting)
        {
            return WebConfigurationManager.AppSettings[setting];
        }

        public static object GetSection(string section)
        {
            return WebConfigurationManager.GetWebApplicationSection(section);
        }
    }
}
