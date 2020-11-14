using System.Collections.Generic;

namespace GoogleForADay.Core.Model.Store
{
    public class Keyword : Entity
    {
        public string Term { get; set; }

        public List<Reference> References { get; set; }

        public override int GetHashCode()
        {
            return Term.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Keyword kObj) && kObj.Term == Term;
        }
    }
}
