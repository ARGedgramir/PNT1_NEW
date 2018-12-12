using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appIMDB.Models
{
    public abstract class Entity
    {
        public virtual int Id
        {
            get;
            set;
        }

        public abstract Type EntityType
        {
            get;
        }

        public virtual bool IsPersisted
        {
            get { return this.Id != 0; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!this.IsPersisted)
            {
                return false;
            }

            var that = obj as Entity;
            if (that == null)
            {
                return false;
            }

            if (this.EntityType != that.EntityType)
            {
                return false;
            }

            return object.Equals(this.Id, that.Id);
        }

        public override int GetHashCode()
        {
            return this.IsPersisted ? this.Id.GetHashCode() : base.GetHashCode();
        }
    }
}