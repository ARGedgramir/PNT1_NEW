using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace appIMDB.Models
{
    public class Actor : Entity
    {
        private ISet<MovieRole> movieRoles;

        public override Type EntityType
        {
            get { return typeof(Actor); }
        }

        public virtual string Name { get; set; }

        public virtual int Age { get
            {
                TimeSpan interval = DateTime.Today - BirthDate;
                return interval.Days / 365;
            }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] 
        [Display(Name = "Birth Date")]
        public virtual DateTime BirthDate { get; set; }

        public virtual string Nationality { get; set; }

        [Display(Name = "Roles")]
        public virtual ISet<MovieRole> MovieRoles
        {
            get
            {
                if (movieRoles == null)
                {
                    movieRoles = new HashSet<MovieRole>();
                }

                return movieRoles;
            }
            set
            {
                movieRoles = value;
            }
        }
    }
}