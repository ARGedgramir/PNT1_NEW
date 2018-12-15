using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appIMDB.Models;
using appIMDB.NHibernate;
using NHibernate;
using appIMDB.Mappers;


namespace appIMDB.Controllers
{
    public class MovieController : Controller
    {
        private readonly ISession session;
        public MovieController()
        {
            this.session = SessionFactory.Instance.OpenSession();
            this.session.Transaction.Begin();
        }
        // GET: Movie
        public ActionResult Index(string searchString)
        {
            var movies = String.IsNullOrEmpty(searchString)
                ? this.session.Query<Movie>().ToList()
                : this.session.Query<Movie>().Where(a => a.Title.Contains(searchString)).ToList();
            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            Movie movie = this.session.Get<Movie>(id);
            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie newMovie)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    this.session.Save(newMovie);
                    this.session.Transaction.Commit();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", "Error creating actor: " + ex.Message);
            }
            return View (newMovie);
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            Movie movie = this.session.Get<Movie>(id);
            if (movie == null)
            {
                return this.HttpNotFound();
            }
            ViewBag.Actors = this.session.Query<Actor>();
            return View(movie);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            Movie movieToUpdate = this.session.Get<Movie>(movie.Id);
            if (movie == null)
            {
                return this.HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var roleIds = this.Request.Form.GetValues("MovieRoleId");
                    var titles = this.Request.Form.GetValues("MovieRoleTitle");
                    var actorIds = this.Request.Form.GetValues("MovieRoleActor");

                    if (titles != null || actorIds != null)
                    {
                        for (int index = 0; index < titles.Length; ++index)
                        {
                            movie.MovieRoles.Add(new MovieRole
                            {
                                Id = int.Parse(roleIds[index]),
                                Movie = movie,
                                Title = titles[index],
                                Actor = session.Get<Actor>(int.Parse(actorIds[index]))
                            });
                        }
                    }

                    MovieMapper.MapFromView(movie, movieToUpdate, session);
                    this.session.Transaction.Commit();
                    return RedirectToAction("Index");
                }

                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", "Error updating movie: " + ex.ToString());
                }
            }
            ViewBag.Actors = this.session.Query<Actor>();
            return View(movie);
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            Movie movie = this.session.Get<Movie>(id);
            if (movie == null)
            {
                return this.HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            Movie movie = this.session.Get<Movie>(id);
            if (movie == null)
            {
                return this.HttpNotFound();
            }
            try
            {
                this.session.Delete(movie);
                this.session.Transaction.Commit();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", "Error deleting movie: " + ex.Message);
            }
            return View(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.session.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
