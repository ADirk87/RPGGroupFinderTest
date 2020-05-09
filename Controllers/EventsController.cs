using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroupFinderCapstone.Models;
using Microsoft.AspNet.Identity;

namespace GroupFinderCapstone.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private RPGGroupFinderEntities db = new RPGGroupFinderEntities();

        // GET: Events
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var events = db.Events.Where(e => !e.IsHidden || e.HostID == userID);
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        [Authorize(Roles = "GM")]
        public ActionResult Create()
        {
            var evnt = new Event
            {
                HostID = User.Identity.GetUserId()
            };
            return View(evnt);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "GM")]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HostID = new SelectList(db.GMs, "GMID", "Username", @event.HostID);
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "GM")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.HostID = new SelectList(db.GMs, "GMID", "Username", @event.HostID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "GM")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,HostID,Location,StartDate,EndDate,Duration,EventTime,MinimumPlayerCount,MaxPlayerCount,GameType")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HostID = new SelectList(db.GMs, "GMID", "Username", @event.HostID);
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "GM")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            foreach(var EventComment in @event.EventComments.ToList())
            {
                db.EventComments.Remove(EventComment);
            }
            foreach (var PlayerSignup in @event.PlayerSignups.ToList())
            {
                db.PlayerSignups.Remove(PlayerSignup);
            }

            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
