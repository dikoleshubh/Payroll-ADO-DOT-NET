using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll_ADO_DOT_NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Employee Payroll Service Project Using ADO.NET Framework");
            EmployeeRepo repo = new EmployeeRepo(); 
            repo.CheckConnection();
        }
        public static void Inputdata()
        {
            EmployeeRepo repository = new EmployeeRepo();
            EmployeeModel model = new EmployeeModel();
            model.name = "Champa";
            model.address = "Kolhapur";
            model.basic_pay = 70000;
            model.deduction = 500;
            model.department = "Testor";
            model.gender = "M";
            model.phone_number = "9567986354";
            model.net_pay = 73000;
            model.income_tax = 1000;
            model.start_date = DateTime.Now;
            model.taxable_pay = 500;

            repository.AddEmployee(model);

            Console.WriteLine(repository.UpdateSalaryIntoDatabase("Venu", 50000) ? "Update done successfully " : "Update Failed");
            repository.GetEmployeesFromForDateRange("2018 - 05 - 03");
            repository.FindGroupedByGenderData("M");
            Console.ReadKey();
        }
        
    }
}

