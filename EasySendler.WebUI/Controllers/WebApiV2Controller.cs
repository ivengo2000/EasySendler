using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using EasySendler.Models.Controls;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    [System.Web.Http.RoutePrefix("api")]
    public class WebApiV2Controller : ApiController
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();

        public WebApiV2Controller()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RecipientList, DropDownViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.rlId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name + " | " + source.Description));
                cfg.CreateMap<MailSetting, DropDownViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.MailSettingsId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email + " | " + source.Description));
                cfg.CreateMap<Template, DropDownViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.TemplateId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name + " | " + source.Description));
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

            var results = _db.RecipientLists.OrderBy(x => x.rlId).Skip((intStart - 1) * intLimit).Take(intLimit).ProjectTo<DropDownViewModel>().ToList();

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

            var results = _db.MailSettings.OrderBy(x => x.MailSettingsId).Skip((intStart - 1) * intLimit).Take(intLimit).ProjectTo<DropDownViewModel>().ToList();

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

            var results = _db.Templates.OrderBy(x => x.TemplateId).Skip((intStart - 1) * intLimit).Take(intLimit).ProjectTo<DropDownViewModel>().ToList();

            return CreateHttpResponseMessage(JsonConvert.SerializeObject(results));
        }

        private HttpResponseMessage CreateHttpResponseMessage(string results, HttpStatusCode status = HttpStatusCode.OK)
        {
            var response = Request.CreateResponse(status);
            response.Content = new StringContent(results, Encoding.UTF8, "application/json");
            return response;
        }
    }
}