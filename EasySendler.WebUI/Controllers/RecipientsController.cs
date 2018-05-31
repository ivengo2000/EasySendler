using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using EasySendler.Extensions;
using EasySendler.Models.BusinessLogic;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    public class RecipientsController : Controller
    {
        private MySmtpEntities db = new MySmtpEntities();

        public RecipientsController()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Recipient, RecipientsViewModel>()
                    .ForMember(dest => dest.rId, y => y.MapFrom(source => source.RecipientId));
                cfg.CreateMap<RecipientsViewModel, Recipient>()
                    .ForMember(dest => dest.RecipientId, y => y.MapFrom(source => source.rId));
            });
        }

        // GET: Recipients
        public ActionResult Index()
        {
            return View(db.Recipients.ToList());
        }

        // GET: Recipients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipient recipient = db.Recipients.Find(id);
            if (recipient == null)
            {
                return HttpNotFound();
            }
            return View(recipient);
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetDetails(string id)
        {
            int rlId;
            if (!int.TryParse(id, out rlId))
            {
                throw new JsonException("RecipientsController.GetDetails: id must be a number.");
            }

            var result = db.Recipients.Where(x => x.RecipientId == rlId).AsQueryable().ProjectTo<RecipientsViewModel>().FirstOrDefault();

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: Recipients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recipients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecipientId,Email,Name,SureName")] Recipient recipient)
        {
            if (ModelState.IsValid)
            {
                db.Recipients.Add(recipient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Mapper.Map<RecipientsViewModel>(recipient));
        }

        // GET: Recipients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipient recipient = db.Recipients.Find(id);
            if (recipient == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<RecipientsViewModel>(recipient));
        }

        // POST: Recipients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rId,Email,Name,SureName")] RecipientsViewModel recipientsViewModel)
        {
            if (ModelState.IsValid)
            {
                Recipient recipient = db.Recipients.Find(recipientsViewModel.rId);
                if (recipient != null)
                {
                    recipient.Email = recipientsViewModel.Email;
                    recipient.Name = recipientsViewModel.Name;
                    recipient.SureName = recipientsViewModel.SureName;

                    db.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }

            return View(recipientsViewModel);
        }

        // GET: Recipients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipient recipient = db.Recipients.Find(id);
            if (recipient == null)
            {
                return HttpNotFound();
            }
            return View(recipient);
        }

        // POST: Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipient recipient = db.Recipients.Find(id);
            db.Recipients.Remove(recipient);
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
