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
    class Screening
    {
        public int ScreeningNo { get; set; }

        public DateTime ScreeningDate { get; set; }

        public string ScreeningType { get; set; }

        public int SeatsRemaining { get; set; }

        public Cinema Cinema { get; set; }

        public Movie Movie { get; set; }

        public Screening(){}

        public Screening(int sn, DateTime sd, string st, Cinema cnm, Movie m)
        {
            ScreeningNo = sn;
            ScreeningDate = sd;
            ScreeningType = st;
            Cinema = cnm;
            Movie = m;
            SeatsRemaining = Cinema.Capacity;
        }

        public override string ToString()
        {
            return "Screening No.: " + ScreeningNo + " Screening Date: " + ScreeningDate + " Screening Type: " + ScreeningType + " Cinema: " + Cinema + " Movie: " + Movie;
        }
    }
}
