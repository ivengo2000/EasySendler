using System.Collections.Generic;
using System.Linq;
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
        private int descriptionTruncateValue = 85;

        public MailSettingsController()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MailSetting, MailSettingViewModel>()
                    .AfterMap((src, dest) => dest.Password = dest.Password.Decrypt());
                cfg.CreateMap<MailSetting, DropDownViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.MailSettingsId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email + "|" + (source.Description.Length > descriptionTruncateValue ? source.Description.Substring(0, descriptionTruncateValue) + "..." : source.Description)));
                cfg.CreateMap<MailSettingViewModel, MailSetting>()
                    .AfterMap((src, dest) => dest.Password = dest.Password.Encrypt())
                ;
            });
        }

        // GET: MailSettings
        public ActionResult Index()
        {
            return View(_db.MailSettings.ProjectTo<MailSettingViewModel>().OrderBy(x => x.MailSettingsId).ToList());
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

        [HttpGet]
        [JsonExceptionFilter]
        public ActionResult GetEdit(string id)
        {
            int mailSettingsId;
            if (!int.TryParse(id, out mailSettingsId))
            {
                throw new JsonException("MailSettingsController.GetEdit: id must be a number.");
            }

            var result = mailSettingsId == 0
                ? new MailSettingViewModel { MailSettingsId = 0, Email = string.Empty, Description = string.Empty, Password = string.Empty, SMTP = string.Empty, SMTPPort = string.Empty, Pop3 = string.Empty, Pop3Port = string.Empty, Imap = string.Empty, ImapPort = string.Empty, EnableSsl = false }
                : Mapper.Map<MailSettingViewModel>(_db.MailSettings.Where(x => x.MailSettingsId == mailSettingsId).AsQueryable().FirstOrDefault());

            return PartialView(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEdit([Bind(Include = "MailSettingsId,Email,Password,SMTP,SMTPPort,Pop3,Pop3Port,EnableSSL,Imap,ImapPort,Description")] MailSettingViewModel mailSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_db.MailSettings.Any(x => x.Email.Equals(mailSettingViewModel.Email) && x.MailSettingsId != mailSettingViewModel.MailSettingsId))
                {
                    ModelState.AddModelError("Email", "Email must be unique.");

                    return PartialView("GetEdit", mailSettingViewModel);
                }
                
                if (mailSettingViewModel.MailSettingsId == 0)
                {
                    _db.MailSettings.Add(Mapper.Map<MailSetting>(mailSettingViewModel));

                    _db.SaveChanges();
                }
                else
                {
                    var set = _db.MailSettings.Find(mailSettingViewModel.MailSettingsId);
                    if (set != null)
                    {
                        set.Email = mailSettingViewModel.Email;
                        set.Password = mailSettingViewModel.Password.Encrypt();
                        set.SMTP = mailSettingViewModel.SMTP;
                        set.SMTPPort = mailSettingViewModel.SMTPPort;
                        set.Pop3 = mailSettingViewModel.Pop3;
                        set.Pop3Port = mailSettingViewModel.Pop3Port;
                        set.EnableSSL = mailSettingViewModel.EnableSsl;
                        set.Imap = mailSettingViewModel.Imap;
                        set.ImapPort = mailSettingViewModel.ImapPort;
                        set.Description = mailSettingViewModel.Description;

                        _db.SaveChanges();
                    }
                }
            }

            return PartialView(mailSettingViewModel);
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
    }
}