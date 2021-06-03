using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Payroll_ADO_DOT_NET
{
    public class EmployeeRepo
    {
        public static string connectionString = "Data Source=.;Initial Catalog=PAYROLL;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);

        public void CheckConnection()
        {
            try
            {
                using (this.connection)
                {
                    connection.Open();
                    Console.WriteLine("Databased Connected");
                }
            }
            catch
            {
                Console.WriteLine("Database is not connected");
            }
            finally
            {
                this.connection.Close();
            }

        }
    }
}
