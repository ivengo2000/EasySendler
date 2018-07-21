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
        private MySmtpEntities _db = new MySmtpEntities();

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
            return View(_db.Recipients.ToList());
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

            var result = _db.Recipients.Where(x => x.RecipientId == rlId).AsQueryable().ProjectTo<RecipientsViewModel>().FirstOrDefault();

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetDelete(string id)
        {
            int rId;
            if (!int.TryParse(id, out rId))
            {
                throw new JsonException("RecipientsController.GetDelete: id must be a number.");
            }

            object result;
            var lists = _db.RecipientLists.Where(x => x.RecipientListsRelations.Any(r => r.rId == rId)).Select(s => s.Name).ToArray();
            if (lists.Length > 0)
            {
                result = new
                {
                    Error = "This Recipient is part of following Recipient Lists:",
                    ErrorRecipientLists = string.Join(", ", lists),
                    ErrorTip = "Need to exclude it from all lists before to compolete this operation."
                };
            }       
            else
            {
                result = _db.Recipients.Where(x => x.RecipientId == rId).AsQueryable().ProjectTo<RecipientsViewModel>().FirstOrDefault();
            }

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
                : _db.Recipients.Where(x => x.RecipientId == rId).AsQueryable().ProjectTo<RecipientsViewModel>().FirstOrDefault();

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
                    _db.Recipients.Add(Mapper.Map<Recipient>(recipientsViewModel));

                    _db.SaveChanges();
                }
                else
                {
                    Recipient recipient = _db.Recipients.Find(recipientsViewModel.rId);
                    if (recipient != null)
                    {
                        recipient.Email = recipientsViewModel.Email;
                        recipient.Name = recipientsViewModel.Name;
                        recipient.SureName = recipientsViewModel.SureName;

                        _db.SaveChanges();
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
            Recipient recipient = _db.Recipients.Find(id);
            if (recipient != null)
            {
                _db.Recipients.Remove(recipient);

                _db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}