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
    public class ActorController : Controller
    {
        private readonly ISession session;
        public ActorController()
        {
            this.session = SessionFactory.Instance.OpenSession();
            this.session.Transaction.Begin();
        }
        // GET: Actor
        public ActionResult Index(string searchString)
        {
            var actors = String.IsNullOrEmpty(searchString)
                ? this.session.Query<Actor>().ToList()
                : this.session.Query<Actor>().Where(a => a.Name.Contains(searchString)).ToList();
            return View(actors);
        }

        // GET: Actor/Details/5
        public ActionResult Details(int id)
        {
            Actor actor = this.session.Get<Actor>(id);
            return View(actor);
        }

        // GET: Actor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actor/Create
        [HttpPost]
        public ActionResult Create(Actor newActor)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    this.session.Save(newActor);
                    this.session.Transaction.Commit();

                    return this.RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", "Error creating actor: " + ex.Message);
            }
            return View(newActor);
        }

        // GET: Actor/Edit/5
        public ActionResult Edit(int id)
        {
            Actor actor = this.session.Get<Actor>(id);
            if (actor == null)
            {
                return this.HttpNotFound();
            }
            ViewBag.Movies = this.session.Query<Movie>();
            return View(actor);
        }

        // POST: Actor/Edit/5
        [HttpPost]
        public ActionResult Edit(Actor actor)
        {
            Actor actorToUpdate = this.session.Get<Actor>(actor.Id);
            if (actor == null)
            {
                return this.HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var roleIds = this.Request.Form.GetValues("MovieRoleId");
                    var titles = this.Request.Form.GetValues("MovieRoleTitle");
                    var movieIds = this.Request.Form.GetValues("MovieRoleMovie");

                    if (titles != null || movieIds != null)
                    {
                        for (int index = 0; index < titles.Length; ++index)
                        {
                            actor.MovieRoles.Add(new MovieRole
                            {
                                Id = int.Parse(roleIds[index]),
                                Actor = actor,
                                Title = titles[index],
                                Movie = session.Get<Movie>(int.Parse(movieIds[index]))
                            });
                        }
                    }
                    ActorMapper.MapFromView(actor, actorToUpdate, session);
                    this.session.Transaction.Commit();
                    return RedirectToAction("Index");
                }

                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", "Error updating actor: " + ex.ToString());
                }
            }
            ViewBag.Movies = this.session.Query<Movie>();
            return View(actor);
        }

        // GET: Actor/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Actor actor = this.session.Get<Actor>(id);
            if (actor == null)
            {
                return this.HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actor/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            Actor actor = this.session.Get<Actor>(id);
            if (actor == null)
            {
                return this.HttpNotFound();
            }
            try
            {
                this.session.Delete(actor);
                this.session.Transaction.Commit();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", "Error deleting actor: " + ex.Message);
            }
            return View(actor);
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