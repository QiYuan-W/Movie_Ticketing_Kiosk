//============================================================
// Student Number : S10222150B, S10221816B
// Student Name : Wong Qi Yuan, Ernest Toh Wee Kiat
// Module Group : T11
//============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp
{
    class SeniorCitizen:Ticket
    {
        public int YearOfBirth { get; set; }

        public SeniorCitizen() : base() { }

        public SeniorCitizen(Screening scrn, int yob):base(scrn)
        {
            YearOfBirth = yob;
        }

        public override double CalculatePrice()
        {
            if ((Screening.ScreeningDate - Screening.Movie.OpeningDate).TotalDays < 7) //check first 7 day, true --> apply adult
            {
                if (Screening.ScreeningType == "2D") //check screening type
                {
                    if ((int)Screening.ScreeningDate.DayOfWeek >= 5) { return 12.5; } //check day
                    else { return 8.5; }
                }
                else
                {
                    if ((int)Screening.ScreeningDate.DayOfWeek >= 5) { return 14; }
                    else { return 11; }
                }
            }

            else
            {
                if (Screening.ScreeningType == "2D") //check screening type
                {
                    if ((int)Screening.ScreeningDate.DayOfWeek >= 5) { return 12.5; } //check day
                    else { return 5; }
                }
                else
                {
                    if ((int)Screening.ScreeningDate.DayOfWeek >= 5) { return 14; }
                    else { return 6; }
                }
            }
            
        }
        public override string ToString()
        {
            return base.ToString() + " Year of Birth: " + YearOfBirth;
        }
    }
}
