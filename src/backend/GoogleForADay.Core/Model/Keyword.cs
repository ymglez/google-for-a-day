using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleForADay.Core.Model
{
    public class Keyword : Entity
    {
        public string Term { get; set; }

        public List<Reference> References { get; set; }
    }
}
