using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using appIMDB.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace appIMDB.NHibernate
{
    public class MovieRoleMapping : ClassMapping <MovieRole>
    {
        public MovieRoleMapping()
        {
            this.Schema("dbo");
            this.Table("MovieRole");

            this.Id(
                e => e.Id,
                m =>
                {
                    m.Column("Id");
                    m.Generator(Generators.Native);
                });

            this.Property(
                e => e.Title,
                m =>
                {
                    m.Column("Title");
                    m.NotNullable(false);
                    m.Unique(false);
                    m.Length(50);
                });

            this.ManyToOne(
                e => e.Movie,
                m =>
                {
                    m.Update(true);
                    m.NotNullable(true);
                    m.Column("MovieId");
                    m.Unique(false);
                    m.Cascade(Cascade.None);
                });

            this.ManyToOne(
                e => e.Actor,
                m =>
                {
                    m.Update(true);
                    m.NotNullable(true);
                    m.Column("ActorId");
                    m.Unique(false);
                    m.Cascade(Cascade.None);
                });
        }
    }
}