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
        
        [Display(Name = "Nombre y Apellido del actor")]
        [Required(ErrorMessage = "El actor debe tener un nombre y apellido")]
        public virtual string Name { get; set; }
    
        [Display(Name = "Edad")]
        public virtual int Age { get
            {
                TimeSpan interval = DateTime.Today - BirthDate;
                return interval.Days / 365;
            }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "El actor debe tener una fecha de nacimiento")]
        public virtual DateTime BirthDate { get; set; }

        [Display(Name = "Nacionalidad")]
        [Required(ErrorMessage = "El actor debe tener una nacionalidad")]
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