using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReservationDAL
    {
        private string connectionString;
        private string SQLQuery;

        public ReservationDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Reservation> QueryReservations(string parkName, string customerName, DateTime startDate, DateTime endDate)
        {

            List<Reservation> availableReservationList = new List<Reservation>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SQLQuery =


                                "SELECT top 15 park.name as park_name, site.site_id, campground.name, campground.daily_fee FROM site INNER JOIN campground ON campground.campground_id = site.campground_id INNER JOIN park on park.park_id=campground.park_id " +
                                "WHERE park.name = @parkName AND site.site_id NOT IN ( " +
                                    "SELECT reservation.site_id FROM reservation " +
                                    "WHERE ((@endDate >= reservation.from_date) AND (@endDate <= reservation.to_date)) " +
                                    "OR ((@startDate >= reservation.from_date) AND (@startDate <= reservation.to_date)) " +
                                    "OR ((@startDate <= reservation.from_date) AND (@endDate >= reservation.to_date)) " +
                                    "OR (month(@startDate) between campground.open_from_mm and campground.open_to_mm) AND month(@endDate) between campground.open_from_mm and campground.open_to_mm " +
                                    ") " +
                                    "group by park.name, campground.name, site.site_id, campground.daily_fee";



                    //                    SELECT campground.name, site.site_number, campground.daily_fee
                    //                    FROM (
                    //                        SELECT campground.name, site.site_number, campground.daily_fee, rn = ROW_NUMBER()
                    //                        OVER(partition by campground.name order by site.site_number)
                    //                        FROM campground
                    //                        INNER JOIN site on site.campground_id = campground.campground_id
                    //                        WHERE site.site_id NOT IN(
                    //                            SELECT reservation.site_id FROM reservation                    
                    //                            WHERE(('10/14/2016' >= reservation.from_date) AND('10/14/2016' <= reservation.to_date))                    
                    //                            OR(('10/12/2016' >= reservation.from_date) AND('10/12/2016' <= reservation.to_date))                    
                    //                            OR(('10/12/2016' <= reservation.from_date) AND('10/14/2016' >= reservation.to_date))                    
                    //                            OR((month('10/12/2016') between campground.open_from_mm and campground.open_to_mm) AND(month('10/14/2016') between campground.open_from_mm and campground.open_to_mm))
                    //		                    )
                    //	                ) as temp 
                    //                  WHERE rn <= 5                  
                    //                  GROUP BY campground.name, site.site_number, campground.daily_fee;
                    //this doesn't quite work...
                    



                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQLQuery;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@parkName", parkName);
                    cmd.Parameters.AddWithValue("@customerName", customerName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation r = new Reservation();

                        r.parkName = Convert.ToString(reader["park_name"]);
                        r.SiteID = Convert.ToInt32(reader["site_id"]);
                        r.Campground = Convert.ToString(reader["name"]);
                        r.DailyFee = Convert.ToDouble(reader["daily_fee"]);

                        availableReservationList.Add(r);
                    }

                    return availableReservationList;
                }
            }
            catch (SqlException ex)
            {
                //this should probably be expanded
                throw;
            }
        }

        public List<Reservation> GetConflictingReservations(string campgroundName, DateTime startDate, DateTime endDate)
        {

            List<Reservation> reservationList = new List<Reservation>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SQLQuery = "SELECT reservation.site_id FROM reservation " +
                               "WHERE ((@endDate >= reservation.from_date) AND (@endDate <= reservation.to_date)) " +
                               "OR ((@startDate >= reservation.from_date) AND (@startDate <= reservation.to_date)) " +
                               "OR ((@startDate <= reservation.from_date) AND (@endDate >= reservation.to_date))";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQLQuery;
                    cmd.Connection = connection;

                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@campgroundName", campgroundName);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Reservation> conflictList = new List<Reservation>();

                    while (reader.Read())
                    {
                        Reservation conflict = new Reservation();

                        conflict.FromDate = Convert.ToDateTime(reader["from_date"]);
                        conflict.ToDate = Convert.ToDateTime(reader["to_date"]);
                        conflict.SiteID = Convert.ToInt32(reader["site_id"]);

                        conflictList.Add(conflict);
                    }

                    return conflictList;
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public int MakeReservation(int siteID, string customerName, DateTime fromDate, DateTime toDate)
        {
            int confirmationID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SQLQuery = "INSERT INTO reservation (site_id, name, from_date, to_date, create_date)" +
                               "VALUES (@siteID, @name, @fromDate, @toDate, (SELECT CAST(GETDATE() AS DATE))); SELECT CAST(SCOPE_IDENTITY() as int);";
                                        
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQLQuery;
                    cmd.Connection = connection;

                    cmd.CommandText = SQLQuery;
                    cmd.Parameters.AddWithValue("@siteID", siteID);
                    cmd.Parameters.AddWithValue("@name", customerName);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);                    

                    confirmationID = (int)cmd.ExecuteScalar();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return confirmationID;
        }
        public List<Reservation> GetNext30Res()
        {
            SQLQuery = @"select park.name, reservation.site_id, reservation.from_date, reservation.to_date 
                           from reservation 
                           inner join site on site.site_id = reservation.site_id 
                           inner join campground on campground.campground_id = site.campground_id 
                           inner join park on park.park_id = campground.park_id 
                           where from_date <= DATEADD(day, 30, getdate())
						   AND from_date >= GETDATE()";

            List<Reservation> availableReservationList = new List<Reservation>();

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
                        Reservation r = new Reservation();


                        r.FromDate = Convert.ToDateTime(reader["from_date"]);
                        r.ToDate = Convert.ToDateTime(reader["to_date"]);
                        r.SiteID = Convert.ToInt32(reader["site_id"]);
                        r.Name = Convert.ToString(reader["name"]);

                        availableReservationList.Add(r);
                    }
                    
                }
            }
            catch (SqlException ex)
            {
                //this should probably be expanded
                throw;
            }
            return availableReservationList;
        }
    }

}
