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
    class Order
    {
        public int OrderNo { get; set; }

        public DateTime OrderDateTime { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }

        public List<Ticket> TicketList { get; set; } = new List<Ticket>();

        public Order() { }
        public Order(int on, DateTime od)
        {
            OrderNo = on;
            OrderDateTime = od;
            Status = "Unpaid";
        }
        public void AddTicketList(Ticket t)
        {
            TicketList.Add(t);
        }

        public override string ToString()
        {
            return "Order No.: " + OrderNo + " Order Date & Time: " + OrderDateTime;
        }

    }
}
