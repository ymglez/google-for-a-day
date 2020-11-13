using System;

namespace GoogleForADay.Core.Model.Store
{
    public class WebSite : Entity
    {
        public string Url { get; set; }

        public int WordsFounded { get; set; }

        public DateTime LastAccess { get; set; }
    }
}
