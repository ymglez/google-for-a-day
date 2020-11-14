using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleForADay.Core.Model.Store
{
    public class Reference
    {
        public string Url { get; set; }

        public string Tittle { get; set; }

        public int Occurrences { get; set; }


        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Reference reference) && reference.Url == Url;
        }
    }

}
