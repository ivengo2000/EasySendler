using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using EasySendler.Extensions;
using EasySendler.Models.BusinessLogic;
using EasySendler.Models.Controls;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    public class MailSettingsController : Controller
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();

        public MailSettingsController()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MailSetting, MailSettingViewModel>();
                cfg.CreateMap<MailSetting, DropDownViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.MailSettingsId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email + "|" + source.Description));
            });
        }

        // GET: MailSettings
        public ActionResult Index()
        {
            return View(_db.MailSettings.ProjectTo<MailSettingViewModel>().ToList());
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetForDropDown(string searchTerm, int pageSize, int pageNum)
        {
            List<DropDownViewModel> results;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                    results = _db.MailSettings.OrderBy(x => x.MailSettingsId)
                        .Skip((pageNum - 1) * pageSize).Take(pageSize)
                        .ProjectTo<DropDownViewModel>().ToList();
            }
            else
            {
                results = _db.MailSettings.Where(x => x.Email.Contains(searchTerm))
                    .OrderBy(x => x.MailSettingsId).Skip((pageNum - 1) * pageSize).Take(pageSize)
                    .ProjectTo<DropDownViewModel>().ToList();
            }

            var result = new DropDownPagedViewModel
            {
                Results = results,
                Total = _db.MailSettings.Count()
            };

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET: MailSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mailSetting = Mapper.Map<MailSettingViewModel>(_db.MailSettings.Find(id));
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
                _db.MailSettings.Add(mailSetting);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Mapper.Map<MailSettingViewModel>(mailSetting));
        }

        // GET: MailSettings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mailSetting = Mapper.Map<MailSettingViewModel>(_db.MailSettings.Find(id));
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
        public ActionResult Edit([Bind(Include = "MailSettingsId,Email,Password,SMTP,SMTPPort,Pop3,Pop3Port,EnableSSL,Imap,ImapPort,Description")] MailSetting mailSetting)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(mailSetting).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Mapper.Map<MailSettingViewModel>(mailSetting));
        }

        // GET: MailSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mailSetting = Mapper.Map<MailSettingViewModel>(_db.MailSettings.Find(id));
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
            MailSetting mailSetting = _db.MailSettings.Find(id);
            if (mailSetting != null)
            {
                _db.MailSettings.Remove(mailSetting);
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

        public ActionResult GetEdit()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetDetails(string id)
        {
            int mailSettingsId;
            if (!int.TryParse(id, out mailSettingsId))
            {
                throw new JsonException("MailSettingsController.GetDetails: id must be a number.");
            }

            var result = Mapper.Map<MailSettingViewModel>(_db.MailSettings.Find(mailSettingsId));

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //[HttpGet]
        //[JsonExceptionFilter]
        //public ActionResult GetEdit(string id)
        //{
        //    int rlId;
        //    if (!int.TryParse(id, out rlId))
        //    {
        //        throw new JsonException("RecipientListsController.GetDetails: id must be a number.");
        //    }

        //    var result = rlId == 0
        //        ? new RecipientListViewModel { rlId = 0, Name = string.Empty, Description = string.Empty }
        //        : _db.RecipientLists.Where(x => x.rlId == rlId).AsQueryable().ProjectTo<RecipientListViewModel>().FirstOrDefault();

        //    return PartialView(result);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetEdit([Bind(Include = "rlId,Name,Description")] RecipientListViewModel recipientListViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (recipientListViewModel.rlId == 0)
        //        {
        //            _db.RecipientLists.Add(Mapper.Map<RecipientList>(recipientListViewModel));

        //            _db.SaveChanges();
        //        }
        //        else
        //        {
        //            RecipientList recipientList = _db.RecipientLists.Find(recipientListViewModel.rlId);
        //            if (recipientList != null)
        //            {
        //                recipientList.Name = recipientListViewModel.Name;
        //                recipientList.Description = recipientListViewModel.Description;

        //                _db.SaveChanges();
        //            }
        //        }
        //    }

        //    return PartialView(recipientListViewModel);
        //}

        //// POST: RecipientLists/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    RecipientList recipientList = _db.RecipientLists.Find(id);
        //    if (recipientList != null)
        //    {
        //        _db.RecipientLists.Remove(recipientList);

        //        _db.SaveChanges();
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}
