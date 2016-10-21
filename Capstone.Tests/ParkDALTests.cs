using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass()]
    public class ParkDALTests
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Parks;User ID=te_student;Password=techelevator";
        private int parkID = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM reservation; DELETE FROM site; DELETE FROM campground; DELETE FROM park", conn);
                cmd.ExecuteNonQuery();
                
                cmd = new SqlCommand("INSERT INTO Park (name, location, establish_date, area, visitors, description) " +
                                                "VALUES ('TestPark', 'TestState', '2000/01/01', 12345, 123456, 'This is the park desription.'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                parkID = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetParks()
        {
            ParkDAL dal = new ParkDAL(connectionString);

            List<Park> park = dal.GetParks();
            
            Assert.AreEqual("TestPark", park[0].Name);
            Assert.AreEqual("TestState", park[0].Location);
            Assert.AreEqual(12345, park[0].Area);
        }
    }
}
