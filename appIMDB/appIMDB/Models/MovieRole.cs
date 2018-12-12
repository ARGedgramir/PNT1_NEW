using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appIMDB.Models
{
    public class MovieRole : Entity
    {
        public override Type EntityType
        {
            get { return typeof(MovieRole); }
        }

        public virtual string Title
        {
            get;
            set;
        }

        public virtual Movie Movie
        {
            get;
            set;
        }

        public virtual Actor Actor
        {
            get;
            set;
        }
    }
}