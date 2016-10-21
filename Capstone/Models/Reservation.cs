using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int SiteID { get; set; }
        public string parkName { get; set; }
        public string Name { get; set; }
        public string Campground { get; set; }
        public double DailyFee { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }

        //override string when applicable

    }
}
