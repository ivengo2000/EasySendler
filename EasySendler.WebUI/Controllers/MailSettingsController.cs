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
    public class MailSettingsController : Controller
    {
        private MySmtpEntities db = new MySmtpEntities();

        // GET: MailSettings
        public ActionResult Index()
        {
            return View(db.MailSettings.ToList());
        }

        // GET: MailSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSetting mailSetting = db.MailSettings.Find(id);
            if (mailSetting == null)
            {
                return HttpNotFound();
            }
            return View(mailSetting);
        }

        // GET: MailSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MailSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MailSettingsId,Email,Password,SMTP,SMTPPort,Pop3,Pop3Port,EnableSSL,Imap,ImapPort")] MailSetting mailSetting)
        {
            if (ModelState.IsValid)
            {
                db.MailSettings.Add(mailSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mailSetting);
        }

        // GET: MailSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSetting mailSetting = db.MailSettings.Find(id);
            if (mailSetting == null)
            {
                return HttpNotFound();
            }
            return View(mailSetting);
        }

        // POST: MailSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MailSettingsId,Email,Password,SMTP,SMTPPort,Pop3,Pop3Port,EnableSSL,Imap,ImapPort")] MailSetting mailSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mailSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mailSetting);
        }

        // GET: MailSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailSetting mailSetting = db.MailSettings.Find(id);
            if (mailSetting == null)
            {
                return HttpNotFound();
            }
            return View(mailSetting);
        }

        // POST: MailSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MailSetting mailSetting = db.MailSettings.Find(id);
            db.MailSettings.Remove(mailSetting);
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
