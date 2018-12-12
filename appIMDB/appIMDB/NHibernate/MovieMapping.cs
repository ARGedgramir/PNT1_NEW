using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using appIMDB.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace appIMDB.NHibernate
{
    public class MovieMapping : ClassMapping <Movie>
    {
        public MovieMapping ()
        {
            this.Schema("dbo");
            this.Table("Movie");

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

            this.Property(
                e => e.CountryOfOrigin,
                m =>
                {
                    m.Column("CountryOfOrigin");
                    m.NotNullable(true);
                    m.Unique(false);
                    m.Length(50);
                });

            this.Property(
                e => e.ReleaseDate,
                m =>
                {
                    m.Column("ReleaseDate");
                    m.NotNullable(true);
                    m.Unique(false);
                });

            this.Set(
                e => e.MovieRoles,
                cm =>
                {
                    cm.Inverse(true);
                    cm.Lazy(CollectionLazy.Lazy);
                    cm.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    cm.Key(k => k.Column(col => col.Name("MovieId")));
                },
                m => m.OneToMany());
        }
    }
}