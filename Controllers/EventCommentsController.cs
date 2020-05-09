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
    public class EventCommentsController : Controller
    {
        private RPGGroupFinderEntities db = new RPGGroupFinderEntities();

        // GET: EventComments
        public ActionResult Index(int? id)
        {
            var evt = db.Events.Find(id);
            var eventComments = evt.EventComments.OrderBy(c => c.CommentTime);
            ViewBag.EventID = id.Value;
            return PartialView(eventComments.ToList());
        }

        // GET: EventComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComment eventComment = db.EventComments.Find(id);
            if (eventComment == null)
            {
                return HttpNotFound();
            }
            return View(eventComment);
        }

        // GET: EventComments/Create
        public ActionResult Create(int? id)
        {
            var EventComment = new EventComment { EventID = id.Value };
            if (User.IsInRole("GM"))
            {
                EventComment.GMID = User.Identity.GetUserId();
            }
            else
            {
                EventComment.PlayerID = User.Identity.GetUserId();
            }
            ViewBag.Event = db.Events.Find(id);
            return View(EventComment);
        }

        // POST: EventComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventComment eventComment)
        {
            if (ModelState.IsValid)
            {
                eventComment.CommentTime = DateTime.Now;
                db.EventComments.Add(eventComment);
                db.SaveChanges();
                var evt = db.Events.Find(eventComment.EventID);
                if (evt.HostID != User.Identity.GetUserId())
                {
                    var subject = $"Alert for {evt.Location}";
                    string Username;
                    if (User.IsInRole("GM"))
                    {
                        Username = db.GMs.Find(User.Identity.GetUserId()).Username;
                    }
                    else
                    {
                        Username = db.Players.Find(User.Identity.GetUserId()).Username;
                    }
                    var messageBody = $"{Username} commented : \n {eventComment.Comment}";
                    MessageSender.SendEmail(subject, messageBody, evt.GM.Email);
                }
                return RedirectToAction("Details", "Events", new { id = eventComment.EventID });
            }

            ViewBag.Event = db.Events.Find(eventComment.EventID);

            return View(eventComment);
        }

        // GET: EventComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComment eventComment = db.EventComments.Find(id);
            if (eventComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "HostID", eventComment.EventID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", eventComment.PlayerID);
            return View(eventComment);
        }

        // POST: EventComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventCommentID,EventID,PlayerID,Comment,CommentTime")] EventComment eventComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "HostID", eventComment.EventID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", eventComment.PlayerID);
            return View(eventComment);
        }

        // GET: EventComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventComment eventComment = db.EventComments.Find(id);
            if (eventComment == null)
            {
                return HttpNotFound();
            }
            return View(eventComment);
        }

        // POST: EventComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventComment eventComment = db.EventComments.Find(id);
            db.EventComments.Remove(eventComment);
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
