using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkDAL
    {
        private string connectionString;
        private string SQLQuery;

        public ParkDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Park> GetParks()
        {
            SQLQuery = "SELECT * FROM park";
            List<Park> parkList = new List<Park>();

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
                        Park p = new Park();
                        p.Name = Convert.ToString(reader["name"]);
                        p.ParkID = Convert.ToInt32(reader["park_id"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Visitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);
                        
                        parkList.Add(p);
                    }
                }
            }
            catch (SqlException ex)
            {
                //this should probably be expanded
                throw;
            }
            return parkList;
        }
    }
}
