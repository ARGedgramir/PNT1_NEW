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

        public virtual string Title { get; set; }

        [Display(Name = "Country of Origin")]
        public virtual string CountryOfOrigin { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public virtual DateTime ReleaseDate { get; set; }

        [Display(Name = "Roles")]
        public virtual ISet<MovieRole> MovieRoles
        {
            get { return movieRoles ?? (movieRoles = new HashSet<MovieRole>()); }
            set { movieRoles = value; }
        }

    }
}