using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
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
    public class DeliveryController : Controller
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();

        public DeliveryController()
        {
            Mapper.Initialize(cfg =>
            {
                //cfg.CreateMap<RecipientList, RecipientListViewModel>();
                //cfg.CreateMap<RecipientList, DropDownViewModel>()
                //    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.rlId))
                //    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name + "|" + source.Description));
                //cfg.CreateMap<sp_getRecipientsByListIdResult, DualListViewModel>()
                //    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.RecipientId))
                //    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email))
                //    .ForMember(dest => dest.IsSelected, y => y.MapFrom(source => source.Selected));
                //cfg.CreateMap<Recipient, RecipientsViewModel>()
                //    .ForMember(dest => dest.rId, y => y.MapFrom(source => source.RecipientId));
            });
        }
        // GET: Delivery
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult Run(string emaill, string name, string sureName, string templateId, string mailSettingsId)
        {
            int tmplId;
            if (!int.TryParse(templateId, out tmplId))
            {
                throw new JsonException("Delivery.Run: templateId must be a number.");
            }

            int msId;
            if (!int.TryParse(mailSettingsId, out msId))
            {
                throw new JsonException("Delivery.Run: mailSettingsId must be a number.");
            }

            //var recipients = _db.RecipientListsRelations.Where(x => x.rlId.Equals(rlId)).Select(x => x.Recipient).AsQueryable().ProjectTo<RecipientsViewModel>().ToList();

            Thread.Sleep(1000);

            var result = true;

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}