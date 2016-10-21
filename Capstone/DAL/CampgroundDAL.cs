using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        private string connectionString;
        private string SQLQuery;

        public CampgroundDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Campground> GetCampgrounds(int parkID)
        {
            SQLQuery = $"SELECT * FROM campground WHERE campground.park_id = '{parkID}'";
            List<Campground> campgroundList = new List<Campground>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQLQuery;
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground c = new Campground();
                        c.CampgroundID = Convert.ToInt32(reader["campground_id"]);
                        c.ParkID = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.StartMonth = Convert.ToInt32(reader["open_from_mm"]);
                        c.EndMonth = Convert.ToInt32(reader["open_to_mm"]);
                        c.DailyFee = Convert.ToInt32(reader["daily_fee"]);

                        campgroundList.Add(c);
                    }
                }
            }
            catch
            {
                throw;
            }
            return campgroundList;
        }
    }
}
