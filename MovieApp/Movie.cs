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
    class Movie
    {
        public string Title { get; set; }

        public int Duration { get; set; }

        public string Classification { get; set; }

        public DateTime OpeningDate { get; set; }

        public List<string> GenreList { get; set; } = new List<string>();
        public List<Screening> ScreeningList { get; set; } = new List<Screening>();

        public Movie() { }

        public Movie(string t, int d, string cl, DateTime od, List<string> g)
        {
            Title = t;
            Duration = d;
            Classification = cl;
            OpeningDate = od;
            GenreList = g;
        }

        public void AddGenre(string g)
        {
            GenreList.Add(g);
        }
        public void AddScreening(Screening scr)
        {
            ScreeningList.Add(scr);
        }

        public override string ToString()
        {
            return "Title: " + Title + " Duration: " + Duration + " Classification: " + Classification + " Opening Date: " + OpeningDate;
        }
    }
}
