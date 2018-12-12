using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using appIMDB.Models;
using NHibernate;

namespace appIMDB.Mappers
{
    public class MovieMapper
    {
        public static void MapFromView(Movie sourceMovie, Movie destinationMovie, ISession session)
        {
            destinationMovie.Title = sourceMovie.Title;
            destinationMovie.CountryOfOrigin = sourceMovie.CountryOfOrigin;
            destinationMovie.ReleaseDate = sourceMovie.ReleaseDate;

            var actorIds = sourceMovie.MovieRoles.Select(r => r.Actor.Id).ToSet();
            var actors = session.Query<Actor>().Where(a => actorIds.Contains(a.Id));
            var actorsById = actors.ToDictionary(a => a.Id, a => a);

            foreach (var sourceRole in sourceMovie.MovieRoles)
            {
                MovieRole destinationRole;
                if (sourceRole.Id == 0)
                {
                    destinationRole = new MovieRole { };
                    destinationMovie.MovieRoles.Add(destinationRole);
                }
                else
                {
                    destinationRole = destinationMovie.MovieRoles.Single(r => r.Id == sourceRole.Id);
                }
                destinationRole.Title = sourceRole.Title;
                destinationRole.Movie = destinationMovie;
                destinationRole.Actor = actorsById[sourceRole.Actor.Id];
            }

            destinationMovie.MovieRoles.RemoveWhere(destinationRole => destinationRole.IsPersisted && !sourceMovie.MovieRoles.Any(sourceRole => object.Equals(sourceRole, destinationRole)));
        }
    }
}