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
        /// UC6 Getting the detail of salary ofthe employee joining grouped by gender and searched for a particular gender.

        public void FindGroupedByGenderData(string gender)
        {
            SqlConnection connection = new SqlConnection(connectionString); //Connection  object along with string as parameter

           
            try
            {
                using (connection)
                {
                    string query = @"select Gender,count(basic_pay) as EmpCount,min(basic_pay) as MinSalary,max(basic_pay) 
                                   as MaxSalary,sum(basic_pay) as SalarySum,avg(basic_pay) as AvgSalary from dbo.employee_payroll
                                   where Gender=@parameter group by Gender";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@parameter", gender);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int empCount = reader.GetInt32(1);
                            double minSalary = reader.GetDouble(2);
                            double maxSalary = reader.GetDouble(3);
                            double sumOfSalary = reader.GetDouble(4);
                            double avgSalary = reader.GetDouble(5);
                            Console.WriteLine($"Gender:{gender}\nEmployee Count:{empCount}\nMinimum Salary:{minSalary}\nMaximum Salary:{maxSalary}\n" +
                                $"Total Salary for {gender} :{sumOfSalary}\n" + $"Average Salary:{avgSalary}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data found");
                    }
                    reader.Close();
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
        /// UC7 Inserts data into multiple tables using transactions.
        /// </summary>
        public void InsertIntoMultipleTablesWithTransactions()
        {
            SqlConnection connection = new SqlConnection(connectionString); //Connection  object along with string as parameter


            Console.WriteLine("Enter EmployeeID");
            int empID = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Name:");
            string empName = Console.ReadLine();

            DateTime startDate = DateTime.Now;

            Console.WriteLine("Enter Address:");
            string address = Console.ReadLine();

            Console.WriteLine("Enter Gender:");
            string gender = Console.ReadLine();

            Console.WriteLine("Enter PhoneNumber:");
            double phonenumber = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Enter BasicPay:");
            int basicPay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Deductions:");
            int deductions = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter TaxablePay:");
            int taxablePay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter Tax:");
            int tax = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter NetPay:");
            int netPay = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter CompanyId:");
            int companyId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter CompanyName:");
            string companyName = Console.ReadLine();

            Console.WriteLine("Enter DeptId:");
            int deptId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter DeptName:");
            string deptName = Console.ReadLine();

            using (connection)
            {
                connection.Open();

                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();

                // Enlist a command in the current transaction.
                SqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;

                try
                {
                    // Execute 1st command
                    command.CommandText = "insert into company values(@company_id,@company_name)";
                    command.Parameters.AddWithValue("@company_id", companyId);
                    command.Parameters.AddWithValue("@company_name", companyName);
                    command.ExecuteScalar();

                    // Execute 2nd command
                    command.CommandText = "insert into employee values(@emp_id,@EmpName,@gender,@phone_number,@address,@startDate,@company_id)";
                    command.Parameters.AddWithValue("@emp_id", empID);
                    command.Parameters.AddWithValue("@EmpName", empName);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@phone_number", phonenumber);
                    command.Parameters.AddWithValue("@address", address);
                    command.ExecuteScalar();

                    // Execute 3rd command
                    command.CommandText = "insert into payroll values(@emp_id,@Basic_Pay,@Deductions,@Taxable_pay,@Income_tax,@Net_pay)";
                    command.Parameters.AddWithValue("@Basic_Pay", basicPay);
                    command.Parameters.AddWithValue("@Deductions", deductions);
                    command.Parameters.AddWithValue("@Taxable_pay", taxablePay);
                    command.Parameters.AddWithValue("@Income_tax", tax);
                    command.Parameters.AddWithValue("@Net_pay", netPay);
                    command.ExecuteScalar();

                    // Execute 4th command
                    command.CommandText = "insert into department values(@dept_id,@dept_name)";
                    command.Parameters.AddWithValue("@dept_id", deptId);
                    command.Parameters.AddWithValue("@dept_name", deptName);
                    command.ExecuteScalar();

                    // Execute 5th command
                    command.CommandText = "insert into employee_dept values(@emp_id,@dept_id)";
                    command.ExecuteNonQuery();

                    // Commit the transaction after all commands.
                    sqlTran.Commit();
                    Console.WriteLine("All records were added into the database.");
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Console.WriteLine(ex.Message);
                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                       
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        }
    }

}

