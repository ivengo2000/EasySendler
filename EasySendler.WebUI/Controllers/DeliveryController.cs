using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using BuisenessLogicLayer.Abstractions;
using BuisenessLogicLayer.Models;
using BuisenessLogicLayer.Services;
using EasySendler.Extensions;
using EasySendler.Models.BusinessLogic;
using EasySendler.Models.Controls;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();


       // private IMailService _mailService;

        private readonly IMailService _mailService;
       // private readonly IMailMessageService _mailMessageService;

        public DeliveryController()
        {
            _mailService = new MailService();
          //  _mailMessageService = new MailMessageService();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MailSetting, MailSettingViewModel>();
                cfg.CreateMap<Template, TemplateViewModel>();
                //cfg.CreateMap<RecipientList, RecipientListViewModel>();
                //cfg.CreateMap<RecipientList, DropDownViewModel>()
                //    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.rlId))
                //    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name + "|" + source.Description));
                //cfg.CreateMap<sp_getRecipientsByListIdResult, DualListViewModel>()
                //    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.RecipientId))
                //    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email))
                //    .ForMember(dest => dest.IsSelected, y => y.MapFrom(source => source.Selected));
                cfg.CreateMap<Recipient, RecipientsViewModel>()
                   .ForMember(dest => dest.rId, y => y.MapFrom(source => source.RecipientId));
            });
        }
        // GET: Delivery
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult Run(string email, string name, string sureName, string templateId, string mailSettingsId)
        {
            #region Get and Set settings for email that will be sent

            int msId;
            if (!int.TryParse(mailSettingsId, out msId))
            {
                throw new JsonException("Delivery.Run: mailSettingsId must be a number.");
            }

            var mailSetting = Mapper.Map<MailSettingViewModel>(_db.MailSettings.Find(msId));

            int port;
            if (!int.TryParse(mailSetting.SMTPPort, out port))
            {
                throw new JsonException("Delivery.Run: port must be a number.");
            }

            var mailServerInfo = new MailServerInfo
            {
                EnableSsl = mailSetting.EnableSsl,
                Name = mailSetting.SMTP,
                Password = mailSetting.Password,
                Port = port,
                UserName = mailSetting.Email
            };

            _mailService.SetSmtpSettings(mailServerInfo);

            #endregion Get and Set settings for email that will be sent

            int tmplId;
            if (!int.TryParse(templateId, out tmplId))
            {
                throw new JsonException("Delivery.Run: templateId must be a number.");
            }

            var template = Mapper.Map<TemplateViewModel>(_db.Templates.Find(tmplId));


            var generatedBody = template.Body.Replace("{{name}}", name + " " + sureName).Replace("{{email}}", email);


            var emailMessage = new MailMessage(mailSetting.Email, email, template.Name, generatedBody) //ToDo: add new filed into Template entity named Subject.
            {
                IsBodyHtml = true
            };

            var resultMessage = _mailService.SendMail(emailMessage);
            var result = new ErrorInfo
            {
                ResultMessage = resultMessage,
                IsSuccessful = resultMessage.Equals("Message has been sent")
            };

            // Thread.Sleep(1000);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}