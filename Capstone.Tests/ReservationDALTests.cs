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
    public class ReservationDALTests
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

                SqlCommand cmd = new SqlCommand("sql goes here", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("sql goes here", conn);
                parkID = (int)cmd.ExecuteScalar(); //make sure you need to do this
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        //query reservations
        //get conflicting reservations
        //make reservation

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
