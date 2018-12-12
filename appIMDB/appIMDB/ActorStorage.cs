using appIMDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appIMDB
{
    public class ActorStorage
    {
        private static readonly ISet<Actor> Actors = new HashSet<Actor>();

        private static Actor Clone(Actor sourceActor)
        {
            Actor newActor = new Actor();
            newActor.Id = sourceActor.Id;
            Map(sourceActor, newActor);
            return newActor;
        }

        private static void Map(Actor sourceActor, Actor destinationActor)
        {
            destinationActor.Nationality = sourceActor.Nationality;
            destinationActor.Name = sourceActor.Name;
            destinationActor.BirthDate = sourceActor.BirthDate;
        }

        public static Actor GetById(int id)
        {
            return Clone(Actors.FirstOrDefault(m => m.Id == id));
        }

        public static ISet<Actor> GetAll()
        {
            var result = new HashSet<Actor>(Actors.Select(Clone));
            return result;
        }

        public static void Save(Actor updatedActor)
        {
            Actor storageActor = Actors.FirstOrDefault(m => m.Id == updatedActor.Id);

            if (storageActor != null)
            {
                Map(updatedActor, storageActor);

            }
            else
            {
                updatedActor.Id = (Actors.Count == 0 ? 0 : Actors.Max(m => m.Id)) + 1;
                Actors.Add(Clone(updatedActor));
            }

        }

        public static ISet<Actor> GetByName(string name)
        {
            var result = new HashSet<Actor>(Actors.Where(m => m.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(m => m.Name)
                .ThenBy(m => m.Id)
                .Select(Clone));
            return result;
            //Actors.Where(m => m.Name.ToLower().Contains(name.ToLower()) || m.Name.ToUpper().Contains(name.ToUpper()))

        }

        public static void Delete(int id)
        {
            var actor = Actors.FirstOrDefault(m => m.Id == id);
            if (actor != null)
            {
                Actors.Remove(actor);
            }
        }
    }
}