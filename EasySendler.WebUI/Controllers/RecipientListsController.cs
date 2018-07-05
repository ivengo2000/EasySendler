using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using EasySendler.Extensions;
using EasySendler.Models.BusinessLogic;
using EasySendler.Models.Controls;
using EntityFrameworkExtras.EF6;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    public class RecipientListsController : Controller
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();
        private int descriptionTruncateValue = 85;

        public RecipientListsController()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RecipientList, RecipientListViewModel>()
                    .ForMember(dest => dest.Description, y => y.MapFrom(source => source.Description.Length > descriptionTruncateValue ? source.Description.Substring(0, descriptionTruncateValue) + "..." : source.Description));
                cfg.CreateMap<RecipientListViewModel, RecipientList>();
                cfg.CreateMap<RecipientList, DropDownViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.rlId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name + "|" + (source.Description.Length > descriptionTruncateValue ? source.Description.Substring(0, descriptionTruncateValue) + "..." : source.Description)));
                cfg.CreateMap<sp_getRecipientsByListIdResult, DualListViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.RecipientId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email))
                    .ForMember(dest => dest.IsSelected, y => y.MapFrom(source => source.Selected));
                cfg.CreateMap<Recipient, RecipientsViewModel>()
                    .ForMember(dest => dest.rId, y => y.MapFrom(source => source.RecipientId));
            });
        }

        // GET: RecipientLists
        public ActionResult Index()
        {
            return View(_db.RecipientLists.ProjectTo<RecipientListViewModel>().ToList());
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetDetails(string id)
        {
            int rlId;
            if (!int.TryParse(id, out rlId))
            {
                throw new JsonException("RecipientListsController.GetDetails: id must be a number.");
            }

            var result = Mapper.Map<RecipientListViewModel>(_db.RecipientLists.Find(rlId));

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
            int rlId;
            if (!int.TryParse(id, out rlId))
            {
                throw new JsonException("RecipientListsController.GetDetails: id must be a number.");
            }

            var result = rlId == 0
                ? new RecipientListViewModel { rlId = 0, Name = string.Empty, Description = string.Empty }
                : _db.RecipientLists.Where(x => x.rlId == rlId).AsQueryable().ProjectTo<RecipientListViewModel>().FirstOrDefault();

            return PartialView(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEdit([Bind(Include = "rlId,Name,Description")] RecipientListViewModel recipientListViewModel)
        {
            if (ModelState.IsValid)
            {
                if (recipientListViewModel.rlId == 0)
                {
                    _db.RecipientLists.Add(Mapper.Map<RecipientList>(recipientListViewModel));

                    _db.SaveChanges();
                }
                else
                {
                    RecipientList recipientList = _db.RecipientLists.Find(recipientListViewModel.rlId);
                    if (recipientList != null)
                    {
                        recipientList.Name = recipientListViewModel.Name;
                        recipientList.Description = recipientListViewModel.Description;

                        _db.SaveChanges();
                    }
                }
            }

            return PartialView(recipientListViewModel);
        }

        // POST: RecipientLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecipientList recipientList = _db.RecipientLists.Find(id);
            if (recipientList != null)
            {
                _db.RecipientLists.Remove(recipientList);

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: ConfigureList
        public ActionResult ConfigureList()
        {
            return View();
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetRecipientListForDropDown(string searchTerm, int pageSize, int pageNum)
        {
            List<DropDownViewModel> results;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                results = _db.RecipientLists.OrderBy(x => x.rlId)
                    .Skip((pageNum - 1)*pageSize).Take(pageSize)
                    .ProjectTo<DropDownViewModel>().ToList();
            }
            else
            {
                results = _db.RecipientLists.Where(x => x.Name.Contains(searchTerm))
                    .OrderBy(x => x.rlId).Skip((pageNum - 1)*pageSize).Take(pageSize)
                    .ProjectTo<DropDownViewModel>().ToList();
            }

            var result = new DropDownPagedViewModel
            {
                Results = results,
                Total = _db.RecipientLists.Count()
            };

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetRecipientsToConfigure(string id)
        {
            int rlId;
            if (!int.TryParse(id, out rlId))
            {
                throw new JsonException("GetRecipientsToConfigure: id must be a number.");
            }

            var result = _db.sp_getRecipientsByListId(rlId).AsQueryable().ProjectTo<DualListViewModel>().ToList();

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetRecipients(string id)
        {
            int rlId;
            if (!int.TryParse(id, out rlId))
            {
                throw new JsonException("GetRecipientsToConfigure: id must be a number.");
            }

            var result = _db.RecipientListsRelations.Where(x => x.rlId.Equals(rlId)).Select(x => x.Recipient).AsQueryable().ProjectTo<RecipientsViewModel>().ToList();

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [JsonExceptionFilter]
        public JsonResult GetRecipientsAmount(string id)
        {
            int rlId;
            if (!int.TryParse(id, out rlId))
            {
                throw new JsonException("GetRecipientsToConfigure: id must be a number.");
            }

            var result = _db.sp_getRecipientCountByListId(rlId);

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject(result),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [JsonExceptionFilter]
        public JsonResult SaveConfiguredList(int[] ids, int listId)
        {
            if (listId == 0)
            {
                throw new JsonException("SaveConfiguredList: please select a recipient list from dropdown.");
            }

            if (ids == null)
            {
                var itemsToDelete = _db.RecipientListsRelations.Where(x => x.rlId.Equals(listId));
                _db.RecipientListsRelations.RemoveRange(itemsToDelete);
                _db.SaveChanges();
            }
            else
            {
                var procedure = new SpSaveConfiguredRecipientList()
                {
                    ListId = listId,
                    Ids = ids.Select(id => new UdtIntArray() { Value = id }).ToList()
                };

                _db.Database.ExecuteStoredProcedure(procedure);
            }

            return new JsonResult
            {
                Data = JsonConvert.SerializeObject("OK"),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
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
