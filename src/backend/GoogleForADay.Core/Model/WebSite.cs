using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleForADay.Core.Model
{
    public class WebSite : Entity
    {
        public string Url { get; set; }

        public int WordsFounded { get; set; }

        public DateTime LastAccess { get; set; }
    }
}
