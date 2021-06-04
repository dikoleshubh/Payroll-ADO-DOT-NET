using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll_ADO_DOT_NET
{
    public class EmployeeModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public double basic_pay { get; set; }
        public DateTime start_date { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phone_number { get; set; }
        public string department { get; set; }
        public double deduction { get; set; }
        public double taxable_pay { get; set; }
        public double income_tax { get; set; }
        public double net_pay { get; set; }
    }
}
