using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

         public override string ToString()
        {
            return Name.PadRight(5) + " - Park Name  \n" + Location.PadRight(5) + " - Location \n" + Area.ToString().PadRight(5) + " - sq ft-Area \n" + EstablishDate.ToShortDateString().PadRight(5)+ " - Establish Date \n" + Visitors.ToString().PadRight(5)+ " - Visitors \n" + "\n"+ "-Description- \n" + Description.PadRight(5)+" \n \n";
        }
    }
}
