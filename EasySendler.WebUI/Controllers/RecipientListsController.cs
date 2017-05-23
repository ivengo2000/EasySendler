using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BuisenessLogicLayer;

namespace EasySendler.Controllers
{
    public class RecipientListsController : Controller
    {
        private MySmtpEntities db = new MySmtpEntities();

        // GET: RecipientLists
        public ActionResult Index()
        {
            return View(db.RecipientLists.ToList());
        }

        // GET: RecipientLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipientList recipientList = db.RecipientLists.Find(id);
            if (recipientList == null)
            {
                return HttpNotFound();
            }
            return View(recipientList);
        }

        // GET: RecipientLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecipientLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rlId,Name,Description")] RecipientList recipientList)
        {
            if (ModelState.IsValid)
            {
                db.RecipientLists.Add(recipientList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recipientList);
        }

        // GET: RecipientLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipientList recipientList = db.RecipientLists.Find(id);
            if (recipientList == null)
            {
                return HttpNotFound();
            }
            return View(recipientList);
        }

        // POST: RecipientLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rlId,Name,Description")] RecipientList recipientList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipientList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recipientList);
        }

        // GET: RecipientLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipientList recipientList = db.RecipientLists.Find(id);
            if (recipientList == null)
            {
                return HttpNotFound();
            }
            return View(recipientList);
        }

        // POST: RecipientLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecipientList recipientList = db.RecipientLists.Find(id);
            db.RecipientLists.Remove(recipientList);
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
