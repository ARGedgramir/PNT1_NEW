using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace appIMDB.Models
{
    public class Movie : Entity
    {
        private ISet<MovieRole> movieRoles;

        public override Type EntityType
        {
            get { return typeof(Movie); }
        }

        [Display(Name = "Título")]
        [Required(ErrorMessage ="La película debe tener un título")]
        public virtual string Title { get; set; }

        [Display(Name = "País de origen")]
        [Required(ErrorMessage = "La película debe tener un país de origen")]
        public virtual string CountryOfOrigin { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de estreno")]
        [Required(ErrorMessage = "La película debe tener una fecha de estreno")]
        public virtual DateTime ReleaseDate { get; set; }

        [Display(Name = "Roles")]
        public virtual ISet<MovieRole> MovieRoles
        {
            get { return movieRoles ?? (movieRoles = new HashSet<MovieRole>()); }
            set { movieRoles = value; }
        }

    }
}