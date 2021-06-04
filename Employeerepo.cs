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
        SqlConnection connection = new SqlConnection(connectionString); //Connection  object along with string as parameter

        public void CheckConnection()
        {   //Confirmating the connection has been set
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

        public void AddEmployee(e model)
        {
            try
            {
                using (this.connection)
                {
                    //Creating a stored Procedure for adding employees into database
                    SqlCommand command = new SqlCommand("dbo.Employee_Daata", this.connection);
                    //Command type is set as stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    //Adding values from employeemodel to stored procedure using disconnected architecture
                    //connected architecture will only read the data
                    command.Parameters.AddWithValue("@EmpName", model.name);
                    command.Parameters.AddWithValue("@basic_Pay", model.basic_pay);
                    command.Parameters.AddWithValue("@StartDate", model.start_date);
                    command.Parameters.AddWithValue("@gender", model.gender);
                    command.Parameters.AddWithValue("@phoneNumber", model.phone_number);
                    command.Parameters.AddWithValue("@department", model.department);
                    command.Parameters.AddWithValue("@address", model.address);
                    command.Parameters.AddWithValue("@deductions", model.deduction);
                    command.Parameters.AddWithValue("@taxable_pay", model.taxable_pay);
                    command.Parameters.AddWithValue("@income_tax", model.income_tax);
                    command.Parameters.AddWithValue("@net_pay", model.net_pay);
                    connection.Open();
                    var result = command.ExecuteNonQuery();
                    connection.Close();


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        public bool UpdateSalaryIntoDatabase(string empName, double basicPay)
        {
            SqlConnection connection= new SqlConnection(connectionString);
           
            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = @"update dbo.employee_payroll set basic_pay=@p1 where EmpName=@p2";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@p1", basicPay);
                    command.Parameters.AddWithValue("@p2", empName);
                    var result = command.ExecuteNonQuery();
                    connection.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
