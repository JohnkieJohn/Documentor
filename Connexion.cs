using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentor
{
    public class Connexion
    {
        public SqlConnection GetConnexion()
        {
            //SqlConnection connexion = new SqlConnection(@"Data Source=DESKTOP-BQ9VVE7;Initial Catalog=DocumentorV1;Persist Security Info=True;User ID=Johnkie;Password=lalinea060606;");
            //SqlConnection connexion = new SqlConnection(@"Data Source=DESKTOP-Q49OLSS;Initial Catalog=DocumentorV1;Persist Security Info=True;User ID=Johnkie;Password=lalinea060606;");

            string DbConnect = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(DbConnect);

            connection.Open();
            return connection;
        }
    }
}
