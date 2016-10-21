using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
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

                        "SELECT top 5 park.name as park_name, site.site_id, campground.name, campground.daily_fee FROM site INNER JOIN campground ON campground.campground_id = site.campground_id INNER JOIN park on park.park_id=campground.park_id " +
                        "WHERE park.name = @parkName AND site.site_id NOT IN ( " +
                            "SELECT reservation.site_id FROM reservation " +
                            "WHERE ((@endDate >= reservation.from_date) AND (@endDate <= reservation.to_date)) " +
                            "OR ((@startDate >= reservation.from_date) AND (@startDate <= reservation.to_date)) " +
                            "OR ((@startDate <= reservation.from_date) AND (@endDate >= reservation.to_date)) " +
                            "OR (month(@startDate) between campground.open_from_mm and campground.open_to_mm) AND month(@endDate) between campground.open_from_mm and campground.open_to_mm " +
                            ") " +
                            "group by park.name, campground.name, site.site_id, campground.daily_fee";


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