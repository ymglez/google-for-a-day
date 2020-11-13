using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleForADay.Core.Model
{
    public abstract class Entity
    {
        protected Entity()
        {
            Created = DateTime.Now;
            Modified = DateTime.Now;
        }

        protected DateTime Created { get; set; }

        protected DateTime Modified { get; set; }

    }
}
