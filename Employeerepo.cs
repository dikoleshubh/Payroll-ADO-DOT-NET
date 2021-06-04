using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

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

        }

        public double UpdateSalaryIntoDatabase(string empName)
        {
            throw new NotImplementedException();
        }


        /// UC2 Ability for Employee Payroll Service to retrieve the Employee Payroll from the Database
        public void GetAllEmployee(string query)
        {
            try
            {
                EmployeeModel employeeModel = new EmployeeModel(); //Model class object

                using (this.connection)
                {
                    string query = @"SELECT * FROM employee_payroll "; // Assigning the query to  retrieve the data from the table

                    SqlCommand cmd = new SqlCommand(query, this.connection);

                    this.connection.Open(); // Conncection condition

                    Console.WriteLine("\nDatabased Connected");

                    SqlDataReader dr = cmd.ExecuteReader();//Executing tthe data reader
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            employeeModel.id = dr.GetInt32(0);
                            employeeModel.name = dr.GetString(1);
                            employeeModel.basic_pay = Convert.ToDouble(dr.GetDecimal(2));
                            employeeModel.start_date = dr.GetDateTime(3);
                            employeeModel.gender = dr.GetString(4);
                            employeeModel.address = dr.GetString(5);
                            employeeModel.phone_number = dr.GetString(6);
                            employeeModel.department = dr.GetString(7);
                            employeeModel.deduction = Convert.ToDouble(dr.GetDecimal(8));
                            employeeModel.taxable_pay = Convert.ToDouble(dr.GetDecimal(9));
                            employeeModel.income_tax = Convert.ToDouble(dr.GetDecimal(10));
                            employeeModel.net_pay = Convert.ToDouble(dr.GetDecimal(11));

                            Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", employeeModel.id, employeeModel.name, employeeModel.basic_pay, employeeModel.start_date, employeeModel.gender, employeeModel.address);
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Found");
                    }
                    dr.Close();
                    this.connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }

        public void AddEmployee(EmployeeModel model)
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
        public void GetEmployeesFromForDateRange(string date)
        {
            string query = $@"select * from dbo.PAYROLL where StartDate between cast('{date}' as date) and cast(getdate() as date)";
            GetAllEmployee(query);
        }

    }
}
