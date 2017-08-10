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
using EntityFrameworkExtras.EF6;
using Newtonsoft.Json;

namespace EasySendler.Controllers
{
    public class RecipientListsController : Controller
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();

        public RecipientListsController()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<RecipientList, RecipientListViewModel>();
                cfg.CreateMap<RecipientList, DropDownViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.rlId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name + "|" + source.Description));
                cfg.CreateMap<sp_getRecipientsByListIdResult, DualListViewModel>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.RecipientId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Email))
                    .ForMember(dest => dest.IsSelected, y => y.MapFrom(source => source.Selected));
            });
        }

        // GET: RecipientLists
        public ActionResult Index()
        {
            return View(_db.RecipientLists.ProjectTo<RecipientListViewModel>().ToList());
        }

        // GET: RecipientLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var recipientList = Mapper.Map<RecipientListViewModel>(_db.RecipientLists.Find(id));
            if (recipientList == null)
            {
                return HttpNotFound();
            }
            return View(recipientList);
        }

        // GET: RecipientLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecipientLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "rlId,Name,Description")] RecipientList recipientList)
        {
            if (ModelState.IsValid)
            {
                _db.RecipientLists.Add(recipientList);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Mapper.Map<RecipientListViewModel>(recipientList));
        }

        // GET: RecipientLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recipientList = Mapper.Map<RecipientListViewModel>(_db.RecipientLists.Find(id));
            if (recipientList == null)
            {
                return HttpNotFound();
            }
            return View(recipientList);
        }

        // POST: RecipientLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "rlId,Name,Description")] RecipientList recipientList)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(recipientList).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Mapper.Map<RecipientListViewModel>(recipientList));
        }

        // GET: RecipientLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recipientList = Mapper.Map<RecipientListViewModel>(_db.RecipientLists.Find(id));
            if (recipientList == null)
            {
                return HttpNotFound();
            }

            return View(recipientList);
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
