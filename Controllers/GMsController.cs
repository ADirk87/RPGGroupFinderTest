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
    public class GMsController : Controller
    {
        private RPGGroupFinderEntities db = new RPGGroupFinderEntities();

        // GET: GMs
        public ActionResult Index()
        {
            return View(db.GMs.ToList());
        }

        // GET: GMs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GM gM = db.GMs.Find(id);
            if (gM == null)
            {
                return HttpNotFound();
            }
            return View(gM);
        }

        // GET: GMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GMID,Username,FirstName,LastName,Email,PreferredMedium,Bio,AvatarFilename")] GM gM)
        {
            if (ModelState.IsValid)
            {
                db.GMs.Add(gM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gM);
        }

        // GET: GMs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GM gM = db.GMs.Find(id);
            if (gM == null)
            {
                return HttpNotFound();
            }
            return View(gM);
        }

        // POST: GMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GMID,Username,FirstName,LastName,Email,PreferredMedium,Bio,AvatarFilename")] GM gM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gM);
        }

        // GET: GMs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GM gM = db.GMs.Find(id);
            if (gM == null)
            {
                return HttpNotFound();
            }
            return View(gM);
        }

        // POST: GMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GM gM = db.GMs.Find(id);
            db.GMs.Remove(gM);
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
