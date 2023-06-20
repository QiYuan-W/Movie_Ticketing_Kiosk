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
    class Adult:Ticket
    {
        public bool PopcornOffer { get; set; }

        public Adult() : base() { }

        public Adult(Screening scrn, bool pco):base(scrn)
        {
            PopcornOffer = pco;
        }

        public override double CalculatePrice()
        {
            if (Screening.ScreeningType == "2D")
            {
                if ((int)Screening.ScreeningDate.DayOfWeek >= 5) { return 12.5; }
                else { return 8.5; }
            }
            else
            {
                if ((int)Screening.ScreeningDate.DayOfWeek >= 5) { return 14; }
                else { return 11; }
            }
        }
        public override string ToString()
        {
            return base.ToString() + "Popcorn offer: " + PopcornOffer;
        }
    }
}
