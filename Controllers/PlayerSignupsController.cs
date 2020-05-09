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
    public class PlayerSignupsController : Controller
    {
        private RPGGroupFinderEntities db = new RPGGroupFinderEntities();

        // GET: PlayerSignups
        public ActionResult Index(int? id)
        {
            var evt = db.Events.Find(id);
            var PlayerSignups = evt.PlayerSignups;
            ViewBag.EventID = id.Value;
            return PartialView(PlayerSignups.ToList());
        }

        // GET: PlayerSignups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerSignup playerSignup = db.PlayerSignups.Find(id);
            if (playerSignup == null)
            {
                return HttpNotFound();
            }
            return View(playerSignup);
        }

        // GET: PlayerSignups/Create
        [Authorize(Roles = "Player")]
        public ActionResult Create(int? id)
        {
            var playerSignup = new PlayerSignup { EventID = id.Value, PlayerID = User.Identity.GetUserId() };
            ViewBag.Event = db.Events.Find(id);
            return View(playerSignup);
        }

        // POST: PlayerSignups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Player")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SignupID,PlayerID,EventID,IsApproved,CommentGM,CommentPlayer")] PlayerSignup playerSignup)
        {
            if (ModelState.IsValid)
            {
                db.PlayerSignups.Add(playerSignup);
                db.SaveChanges();
                var evt = db.Events.Find(playerSignup.EventID);
                if (evt.HostID != User.Identity.GetUserId())
                {
                    var subject = $"Alert for {evt.Location}";
                    string Username = db.Players.Find(User.Identity.GetUserId()).Username;
                    
                    var messageBody = $"{Username} submitted a sign-up attempt : \n {playerSignup.CommentPlayer}";
                    MessageSender.SendEmail(subject, messageBody, evt.GM.Email);
                }
                return RedirectToAction("Details", "Events", new { id = playerSignup.EventID });
            }

            ViewBag.Event = db.Events.Find(playerSignup.EventID);
            return View(playerSignup);
        }

        // GET: PlayerSignups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerSignup playerSignup = db.PlayerSignups.Find(id);
            if (playerSignup == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "HostID", playerSignup.EventID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", playerSignup.PlayerID);
            return View(playerSignup);
        }

        // POST: PlayerSignups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SignupID,PlayerID,EventID,IsApproved,CommentGM,CommentPlayer")] PlayerSignup playerSignup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playerSignup).State = EntityState.Modified;
                db.SaveChanges();
                var evt = db.Events.Find(playerSignup.EventID);
                if (evt.HostID != User.Identity.GetUserId())
                {
                    var subject = $"Alert for {evt.Location} sign up";
                    string Username = db.GMs.Find(User.Identity.GetUserId()).Username;
                    string decision; if (playerSignup.IsApproved)
                    {
                        decision = "Approved";
                    }
                    else
                    {
                        decision = "Declined";
                    }

                    var messageBody = $"{Username} {decision} your application : \n {playerSignup.CommentGM}";
                    var player = db.Players.Find(playerSignup.PlayerID);
                    MessageSender.SendEmail(subject, messageBody, player.Email);
                }
                return RedirectToAction("Details", "Events", new { id = playerSignup.EventID });
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "HostID", playerSignup.EventID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", playerSignup.PlayerID);
            return View(playerSignup);
        }

        // GET: PlayerSignups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerSignup playerSignup = db.PlayerSignups.Find(id);
            if (playerSignup == null)
            {
                return HttpNotFound();
            }
            return View(playerSignup);
        }

        // POST: PlayerSignups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlayerSignup playerSignup = db.PlayerSignups.Find(id);
            db.PlayerSignups.Remove(playerSignup);
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
