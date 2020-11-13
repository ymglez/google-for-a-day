using System.Collections.Generic;

namespace GoogleForADay.Core.Model.Store
{
    public class Keyword : Entity
    {
        public string Term { get; set; }

        public List<Reference> References { get; set; }
    }
}
