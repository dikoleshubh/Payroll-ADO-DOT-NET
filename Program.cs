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
            repo.GetAllEmployee();
        }
    }
}
