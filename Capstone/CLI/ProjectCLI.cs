using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.DAL;
using Capstone.Models;
using System.Configuration;
using ProjectDB;

namespace Capstone.CLI
{

    class ProjectCLI
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
        const string Command_AllParks = "1";
        const string Command_AllCampgrounds = "2";
        const string Command_MakeReservation = "3";
        const string Command_Next30Resv = "4";
        const string Command_Quit = "q";

        public void RunCLI()
        {
            PrintMenu();

            while (true)
            {
                string command = Console.ReadLine();

                Console.Clear();

                switch (command.ToLower())
                {
                    case Command_AllParks:
                        GetParks();
                        break;

                    case Command_AllCampgrounds:
                        GetCampgrounds();
                        break;

                    case Command_Quit:
                        Console.WriteLine("Have a pleasant life.");
                        return;

                    case Command_Next30Resv:
                        GetNext30Res();
                        break;

                    case Command_MakeReservation:
                        MakeReservation();
                        break;
                        
                    default:
                        Console.WriteLine("The command provided was not a valid command, please try again.");
                        break;
                }

                PrintMenu();
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("Main Menu Please type in a command");
            Console.WriteLine("1 - Display all Parks");
            Console.WriteLine("2 - Display all Campgrounds");
            Console.WriteLine("3 - Make a Reservation");
            Console.WriteLine("4 - Show reservations in the next 30 days");
            Console.WriteLine("Q - Quit");
            Console.WriteLine();
        }

        private void GetParks()
        {
            ParkDAL dal = new ParkDAL(connectionString);
            List<Park> parks = dal.GetParks();

            if (parks.Count > 0)
            {
                parks.ForEach(park =>
                {
                    Console.WriteLine(park);
                });
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        private void GetCampgrounds()
        {
            int parkID = CLIHelper.GetInteger("Please specify the park number:");

            CampgroundDAL dal = new CampgroundDAL(connectionString);
            List<Campground> campgroundList = dal.GetCampgrounds(parkID);

            if (campgroundList.Count > 0)
            {
                campgroundList.ForEach(camp =>
                {
                    Console.WriteLine(camp);
                });
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        public void MakeReservation()
        {
            string parkName = CLIHelper.GetString("Please enter a park name");
            string customerName = CLIHelper.GetString("Please enter your name");
            DateTime fromDate = CLIHelper.GetDateTime("Please enter a start date for your reservation mm/dd/yyyy");
            DateTime toDate = CLIHelper.GetDateTime("Please enter an end date for your reservation mm/dd/yyyy");

            ReservationDAL dal = new ReservationDAL(connectionString);
            List<Reservation> reservationList = dal.QueryReservations(parkName, customerName, fromDate, toDate);

            if (reservationList.Count == 0)
            {
                reservationList = dal.GetConflictingReservations(parkName, fromDate, toDate);
                Console.WriteLine("We're sorry, but there is already a reservation booked for the following date(s):"); //to improve on step 3 c, tweak here - need to go up menu

                foreach (Reservation conflict in reservationList)
                {
                    Console.WriteLine(conflict.SiteID.ToString() + " " + conflict.FromDate.ToString() + " to " + conflict.ToDate.ToString());
                }
            }
            else
            {
                Console.WriteLine("Please select one of the following site numbers for your reservation:");
                foreach (Reservation availableReservation in reservationList)
                {
                    Console.WriteLine($"site - {availableReservation.SiteID.ToString()}  {availableReservation.Campground.ToString()} {availableReservation.DailyFee.ToString()} Dollars");
                }
                
                //Console.WriteLine("6 - Return to previous menu.");
                int confirmationNum;
                int userChoice = CLIHelper.GetInteger("Please enter your choice here.");
                confirmationNum = dal.MakeReservation(userChoice, customerName, fromDate, toDate);
           
                Console.WriteLine($"Your confirmation number is {confirmationNum}");
             
                return;
            }
        }
        private void GetNext30Res()
        {
            
            ReservationDAL dal = new ReservationDAL(connectionString);
            List<Reservation> reserv = dal.GetNext30Res();
            Console.WriteLine("Campground   Site ID   Start Date                 Final Date \n");
            if (reserv.Count > 0)
            {
                reserv.ForEach(res =>
                {
                    
                    Console.WriteLine(res.Name.ToString() + "       " + res.SiteID.ToString() +"     "+ res.FromDate.ToString() +"        "+ res.ToDate.ToString()+ "\n");
                });
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }
    }
}
