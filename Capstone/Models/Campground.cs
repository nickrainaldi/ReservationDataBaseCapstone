using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundID { get; set; }
        public int ParkID { get; set; }
        public string Name { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int DailyFee { get; set; }

        public override string ToString()
        {
            string startMonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(StartMonth);
            string endMonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(EndMonth);

            return "\n" + Name + "\n" + "Start Month - " + startMonthName + "\n" + "End Month - " + endMonthName + "\n" + "Daily Fee - " + DailyFee + "\n";
        }
    }
}
