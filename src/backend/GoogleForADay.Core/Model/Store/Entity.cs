using System;

namespace GoogleForADay.Core.Model.Store
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
