using appIMDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appIMDB
{
    public class MovieStorage
    {
        private static readonly ISet<Movie> Movies = new HashSet<Movie>();

        private static Movie Clone(Movie sourceMovie)
        {
            Movie newMovie = new Movie();
            newMovie.Id = sourceMovie.Id;
            Map(sourceMovie, newMovie);
            return newMovie;
        }

        private static void Map(Movie sourceMovie, Movie destinationMovie)
        {
            destinationMovie.CountryOfOrigin = sourceMovie.CountryOfOrigin;
            destinationMovie.Title = sourceMovie.Title;
            destinationMovie.ReleaseDate = sourceMovie.ReleaseDate;
        }

        public static ISet<Movie> GetAll()
        {
            var result = new HashSet<Movie>(Movies.Select(Clone));
            return result;
        }

        public static ISet<Movie> GetByName(string title/*, int pageIndex, int pageSize*/)
        {
            var result = new HashSet<Movie>(Movies.Where(m => m.Title.ToLower().Contains(title.ToLower()) || m.Title.ToUpper().Contains(title.ToUpper()))
                .OrderBy(m => m.Title)
                .ThenBy(m => m.Id)
                .Select(Clone));
            return result;
                //Movies.Where(m => m.Title == title)
                //.OrderBy(m => m.Title).ThenBy(m => m.Id)
                //.Skip(pageSize * pageIndex)
                //.Take(pageSize)
                //.Select(Clone);
                //.ToList();
        }
        public static Movie GetById(int id)
        {
            return Clone(Movies.FirstOrDefault(m => m.Id == id));
        }
        public static void Save(Movie updatedMovie)
        {
            Movie storageMovie = Movies.FirstOrDefault(m => m.Id == updatedMovie.Id);

            if (storageMovie != null)
            {
                Map(updatedMovie, storageMovie);
                 
            }
            else
            {
                updatedMovie.Id = (Movies.Count == 0 ? 0 : Movies.Max(m => m.Id)) + 1;

                Movies.Add(Clone(updatedMovie));
            }
            
        }
        public static void Delete(int id)
        {
            var movie = Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                Movies.Remove(movie);
            }
        }
    }
}