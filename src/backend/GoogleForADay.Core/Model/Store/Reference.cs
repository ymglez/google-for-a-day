using System;
using System.Collections.Generic;
using System.Linq;
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
            // simple comparison fails when (www.google.com and www.google.com/)
            return (obj is Reference reference) && 
                   (reference.Url.Replace('/','\0') == Url.Replace('/', '\0'));
        }
    }

}
