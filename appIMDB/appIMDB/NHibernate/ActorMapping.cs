using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using appIMDB.Models;
using global::NHibernate.Mapping.ByCode;
using global::NHibernate.Mapping.ByCode.Conformist;

namespace appIMDB.NHibernate
{
    public class ActorMapping : ClassMapping <Actor>
    {
        public ActorMapping()
        {
            this.Schema("dbo");
            this.Table("Actor");

            this.Id(
                e => e.Id, 
                m => 
                {
                    m.Column("Id");
                    m.Generator(Generators.Native);
                });

            this.Property(
                e => e.Name,
                m =>
                {
                    m.Column("Name");
                    m.NotNullable(false);
                    m.Unique(false);
                    m.Length(50);
                });

            this.Property(
                e => e.BirthDate,
                m =>
                {
                    m.Column("BirthDate");
                    m.NotNullable(false);
                    m.Unique(false);
                });

            this.Property(
                e => e.Nationality,
                m =>
                {
                    m.Column("Nationality");
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
                    cm.Key(k => k.Column(col => col.Name("ActorId")));
                },
                m => m.OneToMany());
        }
    }
}