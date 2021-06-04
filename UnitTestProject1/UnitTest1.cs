using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Payroll_ADO_DOT_NET;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        [TestClass]
        public class UnitTest
        {
            [TestMethod]

            /// UC 4 Given the update salary value check if the database got updated.
            /// </summary>

            public void GivenUpdateSalaryValue_CheckIfTheDatabaseGotUpdated()
            {
                //Arrange
                string empName = "Venu";
                double basicPay = 60000;
                EmployeeRepo repository = new EmployeeRepo();
                EmployeeModel empModel = new EmployeeModel();
                //Act
                repository.UpdateSalaryIntoDatabase(empName, basicPay);
                double expectedPay = repository.UpdateSalaryIntoDatabase(empName);
                //Assert
                Assert.AreEqual(basicPay, expectedPay);
            }
        }
    }
}
