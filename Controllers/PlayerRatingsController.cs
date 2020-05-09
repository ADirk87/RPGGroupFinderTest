using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroupFinderCapstone.Models;

namespace GroupFinderCapstone.Controllers
{
    public class PlayerRatingsController : Controller
    {
        private RPGGroupFinderEntities db = new RPGGroupFinderEntities();

        // GET: PlayerRatings
        public ActionResult Index()
        {
            var playerRatings = db.PlayerRatings.Include(p => p.GM).Include(p => p.Player);
            return View(playerRatings.ToList());
        }

        // GET: PlayerRatings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerRating playerRating = db.PlayerRatings.Find(id);
            if (playerRating == null)
            {
                return HttpNotFound();
            }
            return View(playerRating);
        }

        // GET: PlayerRatings/Create
        public ActionResult Create()
        {
            ViewBag.GMID = new SelectList(db.GMs, "GMID", "Username");
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username");
            return View();
        }

        // POST: PlayerRatings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RatingID,PlayerID,GMID,Comment,Rating,RatingTime")] PlayerRating playerRating)
        {
            if (ModelState.IsValid)
            {
                db.PlayerRatings.Add(playerRating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GMID = new SelectList(db.GMs, "GMID", "Username", playerRating.GMID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", playerRating.PlayerID);
            return View(playerRating);
        }

        // GET: PlayerRatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerRating playerRating = db.PlayerRatings.Find(id);
            if (playerRating == null)
            {
                return HttpNotFound();
            }
            ViewBag.GMID = new SelectList(db.GMs, "GMID", "Username", playerRating.GMID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", playerRating.PlayerID);
            return View(playerRating);
        }

        // POST: PlayerRatings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RatingID,PlayerID,GMID,Comment,Rating,RatingTime")] PlayerRating playerRating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playerRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GMID = new SelectList(db.GMs, "GMID", "Username", playerRating.GMID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", playerRating.PlayerID);
            return View(playerRating);
        }

        // GET: PlayerRatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlayerRating playerRating = db.PlayerRatings.Find(id);
            if (playerRating == null)
            {
                return HttpNotFound();
            }
            return View(playerRating);
        }

        // POST: PlayerRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PlayerRating playerRating = db.PlayerRatings.Find(id);
            db.PlayerRatings.Remove(playerRating);
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
