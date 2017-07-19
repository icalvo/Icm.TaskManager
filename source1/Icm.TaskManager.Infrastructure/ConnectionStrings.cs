using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Icm.TaskManager.Infrastructure
{
    public static class ConnectionStrings
    {
        public static IDbConnection ByName(string connectionStringName)
        {
            var cnxConfig = ConfigurationManager.ConnectionStrings[connectionStringName];
            var factory = DbProviderFactories.GetFactory(cnxConfig.ProviderName);
            var cnx = factory.CreateConnection();
            if (cnx == null)
            {
                return null;
            }

            cnx.ConnectionString = cnxConfig.ConnectionString;
            return cnx;
        }
    }
}