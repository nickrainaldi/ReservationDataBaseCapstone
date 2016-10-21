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

namespace Capstone.DAL.Tests
{
    [TestClass()]
    public class CampgroundSqlDALTests
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

                SqlCommand cmd = new SqlCommand("INSERT INTO Park (name, location, establish_date, area, visitors, description) " +
                                                "VALUES ('TestPark', 'TestState', '2000/01/01', 12345, 123456, 'This is the park desription.'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                parkID = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) " +
                                     $"VALUES ({parkID}, 'Testground', 5, 9, 50);", conn);
                cmd.ExecuteNonQuery();                
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetCampgrounds()
        {
            CampgroundDAL dal = new CampgroundDAL(connectionString);

            List<Campground> campground = dal.GetCampgrounds(parkID);

            Assert.AreEqual("Testground", campground[0].Name);
            Assert.AreEqual(50, campground[0].DailyFee);
        }
    }
}
