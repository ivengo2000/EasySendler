using System.Linq;
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

        [HttpGet]
        [JsonExceptionFilter]
        public ActionResult GetEdit(string id)
        {
            int rId;
            if (!int.TryParse(id, out rId))
            {
                throw new JsonException("RecipientsController.GetDetails: id must be a number.");
            }

            var result = rId == 0 
                ? new RecipientsViewModel {rId = 0, Email = string.Empty, Name = string.Empty, SureName = string.Empty} 
                : db.Recipients.Where(x => x.RecipientId == rId).AsQueryable().ProjectTo<RecipientsViewModel>().FirstOrDefault();

            return PartialView(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEdit([Bind(Include = "rId,Email,Name,SureName")] RecipientsViewModel recipientsViewModel)
        {
            if (ModelState.IsValid)
            {
                if (recipientsViewModel.rId == 0)
                {
                    db.Recipients.Add(Mapper.Map<Recipient>(recipientsViewModel));

                    db.SaveChanges();
                }
                else
                {
                    Recipient recipient = db.Recipients.Find(recipientsViewModel.rId);
                    if (recipient != null)
                    {
                        recipient.Email = recipientsViewModel.Email;
                        recipient.Name = recipientsViewModel.Name;
                        recipient.SureName = recipientsViewModel.SureName;

                        db.SaveChanges();
                    }
                }
            }

            return PartialView(recipientsViewModel);
        }

        // POST: Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipient recipient = db.Recipients.Find(id);
            if (recipient != null)
            {
                db.Recipients.Remove(recipient);

                db.SaveChanges();
            }
            
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