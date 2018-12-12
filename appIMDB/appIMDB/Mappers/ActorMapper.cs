using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using appIMDB.Models;
using NHibernate;

namespace appIMDB.Mappers
{
    public class ActorMapper
    {
        public static void MapFromView(Actor sourceActor, Actor destinationActor, ISession session)
        {
            destinationActor.Name = sourceActor.Name;
            destinationActor.BirthDate = sourceActor.BirthDate;
            destinationActor.Nationality = sourceActor.Nationality;

            var movieIds = sourceActor.MovieRoles.Select(r => r.Movie.Id).ToSet();
            var movies = session.Query<Movie>().Where(m => movieIds.Contains(m.Id));
            var moviesById = movies.ToDictionary(m => m.Id, m => m);

            foreach (var sourceRole in sourceActor.MovieRoles)
            {
                MovieRole destinationRole;
                if (sourceRole.Id == 0)
                {
                    destinationRole = new MovieRole { };
                    destinationActor.MovieRoles.Add(destinationRole);
                }
                else
                {
                    destinationRole = destinationActor.MovieRoles.Single(r => r.Id == sourceRole.Id);
                }
                destinationRole.Title = sourceRole.Title;
                destinationRole.Actor = destinationActor;
                destinationRole.Movie = moviesById[sourceRole.Movie.Id];
            }

            destinationActor.MovieRoles.RemoveWhere(destinationRole => destinationRole.IsPersisted && !sourceActor.MovieRoles.Any(sourceRole => object.Equals(sourceRole, destinationRole)));
        } 
    }
}