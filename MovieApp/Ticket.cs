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
    abstract class Ticket
    {
        public Screening Screening { get; set; }

        public Ticket() { }

        public Ticket(Screening scr)
        {
            Screening = scr;
        }

        public abstract double CalculatePrice();
        public override string ToString()
        {
            return "Screening: " + Screening;
        }
    }
}
