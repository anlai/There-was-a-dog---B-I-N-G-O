using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Configuration;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Bingo.Web.App_Start.EntityFramework_SqlServerCompact), "Start")]

namespace Bingo.Web.App_Start {
    public static class EntityFramework_SqlServerCompact {
        public static void Start() {
            //Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            Database.DefaultConnectionFactory = new SqlConnectionFactory(WebConfigurationManager.ConnectionStrings["BingoContext"].ConnectionString);
        }
    }
}
