//============================================================
// Student Number : S10222150B, S10221816B
// Student Name : Wong Qi Yuan, Ernest Toh Wee Kiat
// Module Group : T11
//============================================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MovieApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //initializing lists
            List<Movie> MovieList = new List<Movie>();
            InitMovie(MovieList);

            List<Cinema> CinemaList = new List<Cinema>();
            InitCinema(CinemaList);

            List<Screening> ScreeningList = new List<Screening>();
            InitScreening(ScreeningList, MovieList, CinemaList);

            List<Order> OrderList = new List<Order>();

            //Execution of Main Menu
            while (true)
            {
                Console.WriteLine("\nMenu\n1. Add a movie screening session\n2. Delete a movie screening session\n3. Order movie ticket(s)\n4. Cancel order of ticket(s)\n5. Recommend Movie\n6. Show seats remaining\n7. Show All Movies\n8. Show All Screenings of a Movie\n9. Top Cinema Charts\n0. Exit");
                Console.Write("Enter an option: ");
                try 
                { 
                    int userIn = Convert.ToInt32(Console.ReadLine());
                    if (userIn == 1) { AddScrnSess(MovieList, CinemaList, ScreeningList); }
                    else if (userIn == 2) { DelScrnSess(ScreeningList); }
                    else if (userIn == 3) { AddOrder(MovieList, ScreeningList, OrderList); }
                    else if (userIn == 4) { CancelOrder(OrderList); }
                    else if (userIn == 5) { RecoMovie(MovieList); }
                    else if (userIn == 6) { AvalSeat(ScreeningList); }
                    else if (userIn == 7) { DisplayMovies(MovieList); }
                    else if (userIn == 8) { DisplayScreening(MovieList); }
                    else if (userIn == 9) { TopCinema(CinemaList, ScreeningList); }
                    else if (userIn == 0) { break; }
                    else { Console.WriteLine("Invalid option! Please try again.\n"); continue; }
                }
                catch(Exception ex) { Console.WriteLine("Message: {0}\n",ex.Message); continue; }
            }
        }
        static void InitMovie(List<Movie> mList)
        {
            string[] MovieCSV = File.ReadAllLines("Movie.csv");
            for (int i = 1; i < MovieCSV.Length; i++)
            {
                string[] data = MovieCSV[i].Split(',');

                if (String.IsNullOrEmpty(data[0])) { continue; } //check if row in CSV file is empty
                else
                {
                    List<string> genreList = new List<string>(); //converting genre into a List<string>
                    string[] genre = data[2].Split('/');
                    foreach (string g in genre) { genreList.Add(g); }

                    mList.Add(new Movie(data[0], Convert.ToInt32(data[1]), data[3], DateTime.ParseExact(data[4], "dd/MM/yyyy", null), genreList));
                }
            }
        }

        static void InitCinema(List<Cinema> cList)
        {
            string[] CinemaCSV = File.ReadAllLines("Cinema.csv");
            for (int i = 1; i < CinemaCSV.Length; i++)
            {
                string[] data = CinemaCSV[i].Split(',');
                cList.Add(new Cinema(data[0], Convert.ToInt32(data[1]), Convert.ToInt32(data[2])));
            }
        }

        static void InitScreening(List<Screening> sList, List<Movie> mList, List<Cinema> cList)
        {
            string[] ScreeningCSV = File.ReadAllLines("Screening.csv");
            for (int i = 1; i < ScreeningCSV.Length; i++)
            {
                string[] data = ScreeningCSV[i].Split(',');

                //check for matching information(string), then assign class object
                Cinema thisC = null;
                foreach (Cinema c in cList) { if (c.Name == data[2] && c.HallNo == Convert.ToInt32(data[3])) { thisC = c; } }

                Movie thisM = null;
                foreach (Movie m in mList) { if (m.Title == data[4]) { thisM = m; } }

                //ParseExact works logically like Convert.ToDateTime, althought convert cannot be used in this context
                sList.Add(new Screening(1000+i, DateTime.ParseExact(data[0], "dd/MM/yyyy h:mmtt", null), data[1], thisC, thisM));
            }
            foreach (Movie m in mList)
            {
                foreach (Screening s in sList)
                {
                    if (m == s.Movie) { m.AddScreening(s); }
                }
            }
        }

        static void DisplayMovies(List<Movie> mList)
        {
            string[] MovieCSV = File.ReadAllLines("Movie.csv");
            string[] header = MovieCSV[0].Split(',');
            Console.WriteLine("{0,-5}{1,-25}{2,-20}{3,-15}{4,-30}{5,-15}", "S/No",header[0], header[1], header[3], header[2], header[4]);
            int count = 1;
            foreach (Movie m in mList) 
            {
                //converting genrelist into a string
                string genre = "";
                foreach (string g in m.GenreList)
                {
                    if (g == m.GenreList[0]) { genre += g; }
                    else { genre += ("/ " + g); }
                }
                Console.WriteLine("{0,-5}{1,-25}{2,-20}{3,-15}{4,-30}{5,-15}", count,m.Title, m.Duration, m.Classification, genre, m.OpeningDate);
                count++;
            }
            Console.WriteLine();
        }

        static Movie DisplayScreening(List<Movie> mList)
        {
            //get a movie
            DisplayMovies(mList);
            Console.Write("Enter a movie S/No: ");
            int userMovie = Convert.ToInt32(Console.ReadLine());
            //print screening list
            Console.WriteLine("{0,-20}{1,-30}{2,-10}{3,-20}{4,-10}{5,-30}", "Screening No.", "Date and Time", "Type", "Cinema", "Hall No.", "Movie");

            foreach (Screening s in mList[userMovie - 1].ScreeningList)
            {
                Console.WriteLine("{0,-20}{1,-30}{2,-10}{3,-20}{4,-10}{5,-30}", s.ScreeningNo, s.ScreeningDate, s.ScreeningType, s.Cinema.Name, s.Cinema.HallNo, s.Movie.Title);
            }
            return mList[userMovie -1];
        }

        static void AddScrnSess(List<Movie> mList, List<Cinema> cList, List<Screening> sList)
        {
            while (true)
            {
                //get movie
                DisplayMovies(mList);
                Console.Write("Enter a movie S/No: ");
                int movInput = Convert.ToInt32(Console.ReadLine());
                Movie userMovie = mList[movInput - 1];

                Console.Write("Enter Screening Type (2D/3D): "); // 2D or 3D only
                string userType = Console.ReadLine();
                if (!(userType == "2D" || userType == "3D"))
                {
                    Console.WriteLine("Please input a valid Screening Type! (2D/3D)");
                    continue;
                }

                Console.Write("Enter Screening Date (dd/mm/yyyy): ");
                string userDate = Console.ReadLine();
                Console.Write("Enter Screening Time (e.g. 8:00PM): ");
                string userTime = Console.ReadLine();

                string getDateTime = userDate + " " + userTime; //combine both date and time

                //converts string into datetime variable
                DateTime userDateTime;
                try { userDateTime = DateTime.ParseExact(getDateTime, "dd/MM/yyyy h:mmtt", null); }
                catch (System.FormatException) { userDateTime = DateTime.ParseExact(getDateTime, "dd/MM/yyyy hh:mmtt", null); }

                if ((userDateTime - userMovie.OpeningDate).TotalDays < 0) //check if screening date is earlier than movie release
                {
                    Console.WriteLine("\nError: Movie Unavaliable.\nPlease enter a later date\n");
                    continue;
                }

                //display Cinemas
                Console.WriteLine("\n{0,-5}{1,-20}{2,-15}{3,-10}", "S/No", "Name", "Hall Number", "Capacity");
                int count = 1;
                foreach (Cinema c in cList)
                {
                    Console.WriteLine("\n{0,-5}{1,-20}{2,-15}{3,-10}",count, c.Name, c.HallNo, c.Capacity);
                    count++;
                }

                //choose Cinema
                Console.Write("Please enter S/No of Cinema Hall: ");
                int cineInput = Convert.ToInt32(Console.ReadLine());
                Cinema userCinema= cList[cineInput - 1];

                //find screenings with closest before and after to given time
                //get the two screening times closest to input time
                Screening ScrnA = null;
                Screening ScrnB = null;
                foreach (Screening s in sList)
                {
                    if (s.Cinema == userCinema)
                    {
                        if (userDateTime == s.ScreeningDate)
                        {
                            Console.WriteLine("\nError: Cinema Hall unavailable at this time!");
                            continue;
                        }
                        if ((userDateTime - s.ScreeningDate).TotalMinutes > 0) //if user DateTime is later than screening date...
                        {
                            if (ScrnA == null) { ScrnA = s; }
                            else if ((s.ScreeningDate - ScrnA.ScreeningDate).TotalMinutes > 0) { ScrnA = s; }
                        }

                        if ((s.ScreeningDate - userDateTime).TotalMinutes > 0) //if user DateTime is earlier than screening date...
                        {
                            if (ScrnB == null) { ScrnB = s; }
                            else if ((ScrnB.ScreeningDate - s.ScreeningDate).TotalMinutes > 0) { ScrnB = s; }
                        }
                    }
                }

                //end time of X
                DateTime userScrnEnd = userDateTime.AddMinutes(userMovie.Duration + 30);

                //create new screening
                Screening userScrn= null;

                if (ScrnA == null && ScrnB == null) { userScrn = new(sList[sList.Count - 1].ScreeningNo + 1, userDateTime, userType, userCinema, userMovie); }

                else if (ScrnA == null)
                {
                    if ((ScrnB.ScreeningDate - userScrnEnd).TotalMinutes >= 0) { userScrn = new(sList[sList.Count - 1].ScreeningNo + 1, userDateTime, userType, userCinema, userMovie); }
                    else {
                        Console.WriteLine("\nError: Cinema Hall unavailable at this time!");
                        continue;
                    }
                }

                else if (ScrnB == null)
                {
                    //end time of A
                    int ScrnDura = ScrnA.Movie.Duration + 30;
                    DateTime ScrnATime = ScrnA.ScreeningDate.AddMinutes(ScrnDura);
                    if ((userDateTime - ScrnATime).TotalMinutes >= 0) { userScrn = new(sList[sList.Count - 1].ScreeningNo + 1, userDateTime, userType, userCinema, userMovie); }
                    else {
                        Console.WriteLine("\nError: Cinema Hall unavailable at this time!");
                        continue;
                    }
                }
                else if (((ScrnB.ScreeningDate - userScrnEnd).TotalMinutes >= 0) && ((userDateTime - ScrnA.ScreeningDate.AddMinutes(ScrnA.Movie.Duration + 30)).TotalMinutes >= 0)) 
                {
                    userScrn = new(sList[sList.Count - 1].ScreeningNo + 1, userDateTime, userType, userCinema, userMovie);
                }

                else
                {
                    Console.WriteLine("\nError: Cinema Hall unavailable at this time!");
                    continue;
                }

                Console.WriteLine("Session creation successful\n");
                userScrn.Movie.AddScreening(userScrn);
                sList.Add(userScrn);
                Console.WriteLine(userScrn.ToString());

                break;
            }
        }

        static void DelScrnSess(List<Screening> sList)
        {
            Console.WriteLine("{0,-20}{1,-30}{2,-10}{3,-20}{4,-10}{5,-30}", "Screening No.", "Date and Time", "Type", "Cinema", "Hall No.", "Movie"); //header
            foreach (Screening s in sList)
            {
                if (s.SeatsRemaining == s.Cinema.Capacity)
                {
                    Console.WriteLine("{0,-20}{1,-30}{2,-10}{3,-20}{4,-10}{5,-30}", s.ScreeningNo, s.ScreeningDate, s.ScreeningType, s.Cinema.Name, s.Cinema.HallNo, s.Movie.Title);
                }
            }

            Console.Write("Enter a Screening No. to DELETE: ");
            int userScrnNo = Convert.ToInt32(Console.ReadLine());

            foreach (Screening s in sList)
            {
                if (s.ScreeningNo == userScrnNo)
                {
                    sList.Remove(s);
                    s.Movie.ScreeningList.Remove(s);
                    Console.WriteLine("\nRemoval Successful");
                    return;
                }
            }
            Console.WriteLine("\nScreening Not Found!\nPlease try again!");

        }

        static void AddOrder(List<Movie> mList, List<Screening> sList, List<Order> oList)
        {
            while (true)
            {
                Movie userM = DisplayScreening(mList);

                //get screening no.
                Console.Write("\nEnter a Screening No. (or 0 to exit): ");
                int userScrnIn = Convert.ToInt32(Console.ReadLine());
                if (userScrnIn == 0) { break; }
                Screening userScrn = null;
                foreach (Screening s in sList) { if (s.ScreeningNo == userScrnIn) { userScrn = s; } }
                if (userScrn == null || userM != userScrn.Movie) { Console.WriteLine("Invalid Screening No., please try again!"); continue; }

                Console.Write("Enter number of ticket(s) to order: ");
                int NoOfOrders = Convert.ToInt32(Console.ReadLine());
                //check no. of seats
                if ((NoOfOrders < 0) || NoOfOrders > userScrn.SeatsRemaining) { Console.WriteLine("Number of seats unavailable of this screening, please try again!");
                    continue;
                }
                //checks classification eligibility
                if (userScrn.Movie.Classification != "G")
                {
                    Console.Write("Do all ticket holders meet the requirements for a {0} movie? (Y/N): ", userScrn.Movie.Classification);
                    string classYN = Console.ReadLine().ToUpper();
                    if (classYN != "Y") { Console.WriteLine("Sorry you are not eligible to purchase.");
                        break;
                    }
                }

                Order newOrder = null;
                if (oList.Count != 0)
                {
                    newOrder = new Order(oList[oList.Count - 1].OrderNo + 1, DateTime.Now);
                }
                else { newOrder = new Order(1, DateTime.Now); }

                //upon entering this loop, purchase must be fully complete, cannot exit without fully purchase
                for(int i = 0; i < NoOfOrders; i++)
                {
                    try
                    {
                        Console.WriteLine("\nOptions for Ticket {0}", i + 1);
                        Console.WriteLine("Ticket Types:\n1. Student\n2. Senior Citizen\n3. Adult");
                        Console.Write("Select ticket type: ");
                        int tickType = Convert.ToInt32(Console.ReadLine());
                        if (!(tickType > 0 && tickType <= 3)) { i--; Console.WriteLine("Please entry a valid number!"); continue; }
                        while (true)
                        {
                            Console.WriteLine();
                            try
                            {
                                if (tickType == 1)
                                {
                                    Console.WriteLine("Level of Education:\n1. Primary\n2. Secondary\n3. Tertiary");
                                    Console.Write("Enter Level of Education: ");
                                    int lvlEduIn = Convert.ToInt32(Console.ReadLine());
                                    string lvlEdu = "";
                                    if (lvlEduIn == 1) { lvlEdu = "Primary"; }
                                    else if (lvlEduIn == 2) { lvlEdu = "Secondary"; }
                                    else if (lvlEduIn == 3) { lvlEdu = "Tertiary"; }
                                    else { Console.WriteLine("Invalid input, please try again"); continue; }
                                    newOrder.AddTicketList(new Student(userScrn, lvlEdu));
                                    break;
                                }
                                if (tickType == 2)
                                {
                                    Console.Write("Enter age: ");
                                    int userAge = Convert.ToInt32(Console.ReadLine());
                                    if (userAge < 55) { Console.WriteLine("Sorry, too young to be senior citizen.\nPurchasing adult ticket instead..."); tickType = 3; continue; }
                                    newOrder.AddTicketList(new SeniorCitizen(userScrn, userAge));
                                    break;
                                }
                                if (tickType == 3)
                                {
                                    Console.Write("Do you want $3 popcorn? (Y/N): ");
                                    string popYN = Console.ReadLine().ToUpper();
                                    if (popYN == "Y") { newOrder.AddTicketList(new Adult(userScrn, true)); }
                                    else if (popYN == "N") { newOrder.AddTicketList(new Adult(userScrn, false)); }
                                    else { Console.WriteLine("Invalid input, try again!"); continue; }
                                    break;
                                }
                            }
                            catch (Exception ex) { Console.WriteLine("Invalid input\nMessage: {0}\nRetrying options for ticket {1}", ex.Message, i + 1); continue; }
                        }

                        userScrn.SeatsRemaining--;
                    }
                    catch { i--; Console.WriteLine("Invalid input, please enter a valid number!");continue; }
                    
                }

                double totalAmt = 0;
                foreach (Ticket t in newOrder.TicketList) { totalAmt += t.CalculatePrice(); }
                Console.WriteLine("Amount payable: {0:c}\n\nPress Enter to Pay.", totalAmt);
                Console.ReadLine();
                newOrder.Amount = totalAmt;
                newOrder.Status = "Paid";
                oList.Add(newOrder);
                Console.WriteLine("Your order number is {0}\nThank you for your purchase",newOrder.OrderNo);
                break;
            }
        }

        static void CancelOrder(List<Order> oList)
        {
            Console.Write("Please enter order number: ");
            int orderIn = Convert.ToInt32(Console.ReadLine());
            Order userOrder = null;
            foreach (Order o in oList)
            {
                if (o.OrderNo == orderIn) { userOrder = o; }
            }
            if (userOrder == null) { Console.WriteLine("Invalid Order No.\nReturning to Main Menu..."); }
            else if ((userOrder.TicketList[0].Screening.ScreeningDate - DateTime.Now).TotalMinutes >= 0 && userOrder.Status != "Cancelled") //check validity of cancellation
            {
                userOrder.TicketList[0].Screening.SeatsRemaining += userOrder.TicketList.Count;
                userOrder.Status = "Cancelled";
                Console.WriteLine("Your tickets have been refunded.\nAmount refunded: {0:c}\nHave a nice day!", userOrder.Amount);
            }
            else { Console.WriteLine("Your screening has ended/ Order already cancelled!\nUnable to Refund!"); }
        }

        static void RecoMovie(List<Movie> mList)
        {
            Dictionary<Movie, int> tickSoldDict = new Dictionary<Movie, int>();
            foreach (Movie m in mList)
            {
                int tickSold = 0;
                foreach (Screening s in m.ScreeningList)
                {
                    tickSold += s.Cinema.Capacity - s.SeatsRemaining;
                }
                tickSoldDict.Add(m,tickSold);
            }
            // Order by values.
            // ... Use LINQ to specify sorting by value.
            var items = from pair in tickSoldDict
                        orderby pair.Value descending
                        select pair;

            // Display results.
            int count = 0;
            Console.WriteLine("\n{0,-10}{1,-30}{2}","Rank","Movie","No. of Tickets Sold");
            foreach (KeyValuePair<Movie, int> movie in items)
            {
                count++;
                Console.WriteLine("{0,-10}{1,-30}{2}", "["+count+"]", movie.Key.Title, movie.Value);
            }
        }

        static void AvalSeat(List<Screening> sList)
        {
            Dictionary<Screening, int> RemainSeatDict = new Dictionary<Screening, int>();
            foreach (Screening s in sList)
            {
                RemainSeatDict.Add(s, s.SeatsRemaining);
            }
            // Order by values.
            // ... Use LINQ to specify sorting by value.
            var items = from pair in RemainSeatDict
                        orderby pair.Value descending
                        select pair;

            // Display results.
            int count = 0;
            Console.WriteLine("\n{0,-10}{1,-20}{2,-30}{3,-10}{4,-20}{5,-10}{6,-30}{7}", "Rank", "Screening No.", "Date and Time", "Type", "Cinema", "Hall No.", "Movie", "Seats Remaining");
            foreach (KeyValuePair<Screening, int> scrn in items)
            {
                count++;
                Console.WriteLine("{0,-10}{1,-20}{2,-30}{3,-10}{4,-20}{5,-10}{6,-30}{7}", "[" + count + "]", scrn.Key.ScreeningNo, scrn.Key.ScreeningDate, scrn.Key.ScreeningType, scrn.Key.Cinema.Name, scrn.Key.Cinema.HallNo, scrn.Key.Movie.Title, scrn.Value);
            }
        }

        static void TopCinema(List<Cinema> cList, List<Screening> sList)
        {
            Dictionary<string, int> CineNameDict = new Dictionary<string, int>();

            //get name of all cinema
            List<string> cNameList = new List<string>();
            foreach (Cinema c in cList)
            {
                if (cNameList.Any(c.Name.Contains)) { continue; }
                else { cNameList.Add(c.Name); }
            }

            foreach (string i in cNameList) { CineNameDict.Add(i,0); }

            foreach(Screening s in sList)
            {
                int tickSold = s.Cinema.Capacity - s.SeatsRemaining;
                CineNameDict[s.Cinema.Name] += tickSold;
            }

            // Order by values.
            // ... Use LINQ to specify sorting by value.
            var items = from pair in CineNameDict
                        orderby pair.Value descending
                        select pair;

            // Display results.
            int count = 0;
            Console.WriteLine("\n{0,-10}{1,-20}{2,-30}", "Rank", "Cinema", "Tickets Sold");
            foreach (KeyValuePair<string, int> cine in items)
            {
                count++;
                Console.WriteLine("{0,-10}{1,-20}{2,-30}", "[" + count + "]", cine.Key, cine.Value);
            }
        }
    }
}
