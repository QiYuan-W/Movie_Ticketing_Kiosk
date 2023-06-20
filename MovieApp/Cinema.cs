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
    class Cinema
    {
        public string Name { get; set; }

        public int HallNo { get; set; }

        public int Capacity { get; set; }

        public Cinema() { }

        public Cinema(string n, int hn, int cap)
        {
            Name = n;
            HallNo = hn;
            Capacity = cap;
        }

        public override string ToString()
        {
            return "Name: " + Name + "HallNo: " + HallNo + "Capacity: " + Capacity;
        }
    }
}
