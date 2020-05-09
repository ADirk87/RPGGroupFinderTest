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
    public class GMRatingsController : Controller
    {
        private RPGGroupFinderEntities db = new RPGGroupFinderEntities();

        // GET: GMRatings
        public ActionResult Index(string id)
        {
            var GM = db.GMs.Find(id);
            var GMratings = GM.GMRatings;
            ViewBag.GMID = id;
            return PartialView(GMratings.ToList());
            
        }

        // GET: GMRatings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GMRating gMRating = db.GMRatings.Find(id);
            if (gMRating == null)
            {
                return HttpNotFound();
            }
            return View(gMRating);
        }

        // GET: GMRatings/Create
        [Authorize(Roles = "Player")]
        public ActionResult Create(string id)
        {
            var GMRating = new GMRating
            {
                GMID = id,
                PlayerID = User.Identity.GetUserId(),
            };

            ViewBag.GM = db.GMs.Find(id);
            return View(GMRating);
        }

        // POST: GMRatings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Player")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GMRating gMRating)
        {
            if (ModelState.IsValid)
            {
                gMRating.RatingTime = DateTime.Now;
                db.GMRatings.Add(gMRating);
                db.SaveChanges();
                return RedirectToAction("Details", "GMs", new { id = gMRating.GMID });
            }

            ViewBag.GM = db.GMs.Find(gMRating.GMID);
            return View(gMRating);
        }

        // GET: GMRatings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GMRating gMRating = db.GMRatings.Find(id);
            if (gMRating == null)
            {
                return HttpNotFound();
            }
            ViewBag.GMID = new SelectList(db.GMs, "GMID", "Username", gMRating.GMID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", gMRating.PlayerID);
            return View(gMRating);
        }

        // POST: GMRatings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RatingID,GMID,PlayerID,Comment,Rating,RatingTime")] GMRating gMRating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gMRating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GMID = new SelectList(db.GMs, "GMID", "Username", gMRating.GMID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Username", gMRating.PlayerID);
            return View(gMRating);
        }

        // GET: GMRatings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GMRating gMRating = db.GMRatings.Find(id);
            if (gMRating == null)
            {
                return HttpNotFound();
            }
            return View(gMRating);
        }

        // POST: GMRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GMRating gMRating = db.GMRatings.Find(id);
            db.GMRatings.Remove(gMRating);
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
