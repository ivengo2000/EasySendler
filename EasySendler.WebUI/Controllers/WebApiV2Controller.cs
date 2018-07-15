using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using BuisenessLogicLayer.Abstractions;
using BuisenessLogicLayer.Models;
using BuisenessLogicLayer.Services;
using EasySendler.Models.BusinessLogic;
using EasySendler.Models.Controls;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    [System.Web.Http.RoutePrefix("api")]
    public class WebApiV2Controller : ApiController
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();
        private readonly IMailService _mailService;
        private int descriptionTruncateValue = 150;

        public WebApiV2Controller()
        {
            _mailService = new MailService();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MailSetting, MailSettingViewModel>();
                cfg.CreateMap<Template, TemplateViewModel>();
                cfg.CreateMap<RecipientList, MobileListViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.rlId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name))
                    .ForMember(dest => dest.Description, y => y.MapFrom(source => source.Description.Length > descriptionTruncateValue ? source.Description.Substring(0, descriptionTruncateValue) + "..." : source.Description));
                cfg.CreateMap<MailSetting, MobileListViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.MailSettingsId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email))
                    .ForMember(dest => dest.Description, y => y.MapFrom(source => source.Description.Length > descriptionTruncateValue ? source.Description.Substring(0, descriptionTruncateValue) + "..." : source.Description));
                cfg.CreateMap<Template, MobileListViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.TemplateId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name))
                    .ForMember(dest => dest.Description, y => y.MapFrom(source => source.Description.Length > descriptionTruncateValue ? source.Description.Substring(0, descriptionTruncateValue) + "..." : source.Description));
            });
        }
        
        // GET: api/rl/get?limit=10&start=1
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("rl/get")]
        public HttpResponseMessage GetRecipientLists(string limit, string start)
        {
            int intLimit;
            if (!int.TryParse(limit, out intLimit))
            {
                return CreateHttpResponseMessage("The 'limit' parameter must be an integer", HttpStatusCode.BadRequest);
            }

            int intStart = 1;
            if (!int.TryParse(start, out intStart))
            {
                return CreateHttpResponseMessage("The 'start' parameter must be an integer", HttpStatusCode.BadRequest);
            }

            var results = _db.RecipientLists.OrderBy(x => x.rlId).Skip((intStart - 1) * intLimit).Take(intLimit).ProjectTo<MobileListViewModel>().ToList();

            return CreateHttpResponseMessage(JsonConvert.SerializeObject(results));
        }

        // GET: api/s/get?limit=10&start=1
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("s/get")]
        public HttpResponseMessage GetEmailSettings(string limit, string start)
        {
            int intLimit;
            if (!int.TryParse(limit, out intLimit))
            {
                return CreateHttpResponseMessage("The 'limit' parameter must be an integer", HttpStatusCode.BadRequest);
            }

            int intStart = 1;
            if (!int.TryParse(start, out intStart))
            {
                return CreateHttpResponseMessage("The 'start' parameter must be an integer", HttpStatusCode.BadRequest);
            }

            var results = _db.MailSettings.OrderBy(x => x.MailSettingsId).Skip((intStart - 1) * intLimit).Take(intLimit).ProjectTo<MobileListViewModel>().ToList();

            return CreateHttpResponseMessage(JsonConvert.SerializeObject(results));
        }

        // GET: api/t/get?limit=10&start=1
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("t/get")]
        public HttpResponseMessage GetTemplates(string limit, string start)
        {
            int intLimit;
            if (!int.TryParse(limit, out intLimit))
            {
                return CreateHttpResponseMessage("The 'limit' parameter must be an integer", HttpStatusCode.BadRequest);
            }

            int intStart = 1;
            if (!int.TryParse(start, out intStart))
            {
                return CreateHttpResponseMessage("The 'start' parameter must be an integer", HttpStatusCode.BadRequest);
            }

            var results = _db.Templates.OrderBy(x => x.TemplateId).Skip((intStart - 1) * intLimit).Take(intLimit).ProjectTo<MobileListViewModel>().ToList();

            return CreateHttpResponseMessage(JsonConvert.SerializeObject(results));
        }

        // GET: api/run/get?rid=3&tid=1&sid=1
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("run/get")]
        public HttpResponseMessage Run(string rid, string tid, string sid)
        {
            int rlId;
            if (!int.TryParse(rid, out rlId))
            {
                throw new JsonException("Api.Run: rid must be a number.");
            }

            int tmplId;
            if (!int.TryParse(tid, out tmplId))
            {
                throw new JsonException("Api.Run: tid must be a number.");
            }

            int msId;
            if (!int.TryParse(sid, out msId))
            {
                throw new JsonException("Api.Run: sid must be a number.");
            }

            var mailSetting = Mapper.Map<MailSettingViewModel>(_db.MailSettings.Find(msId));

            int port;
            if (!int.TryParse(mailSetting.SMTPPort, out port))
            {
                throw new JsonException("Api.Run: port must be a number.");
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

            var template = Mapper.Map<TemplateViewModel>(_db.Templates.Find(tmplId));

            var recipientsList = _db.RecipientLists.Where(x => x.rlId == rlId).SelectMany(y => y.RecipientListsRelations.Select(r=>r.Recipient)).ToList();

            var result = new List<ErrorInfo>();

            foreach (var recipient in recipientsList)
            {
                var generatedBody = template.Body.Replace("{{name}}", recipient.Name + " " + recipient.SureName).Replace("{{email}}", recipient.Email);

                var emailMessage = new MailMessage(mailSetting.Email, recipient.Email, template.Name, generatedBody) //ToDo: add new filed into Template entity named Subject.
                {
                    IsBodyHtml = true
                };

                var resultMessage = _mailService.SendMail(emailMessage);

                var resultLine = new ErrorInfo
                {
                    ResultMessage = resultMessage,
                    IsSuccessful = resultMessage.Equals("Message has been sent")
                };

                result.Add(resultLine);
            }

            return CreateHttpResponseMessage(JsonConvert.SerializeObject(result));
        }

        private HttpResponseMessage CreateHttpResponseMessage(string results, HttpStatusCode status = HttpStatusCode.OK)
        {
            var response = Request.CreateResponse(status);
            response.Content = new StringContent(results, Encoding.UTF8, "application/json");
            return response;
        }
    }
}