using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using EasySendler.Models.BusinessLogic;
using EasySendler.Models.Controls;
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
                cfg.CreateMap<RecipientList, DropDownResult>()
                    .ForMember(dest => dest.Id, y => y.MapFrom(source => source.rlId))
                    .ForMember(dest => dest.Text, y => y.MapFrom(source => source.Name + "|" + source.Description));
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
            return View(_db.RecipientLists.ProjectTo<RecipientListViewModel>().ToList());
        }

        [HttpGet]
        public JsonResult GetRecipientListForDropDown(string searchTerm, int pageSize, int pageNum)
        {
            List<DropDownResult> results;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                results = _db.RecipientLists.OrderBy(x => x.rlId)
                    .Skip((pageNum - 1)*pageSize).Take(pageSize)
                    .ProjectTo<DropDownResult>().ToList();
            }
            else
            {
                results = _db.RecipientLists.Where(x => x.Name.Contains(searchTerm))
                    .OrderBy(x => x.rlId).Skip((pageNum - 1)*pageSize).Take(pageSize)
                    .ProjectTo<DropDownResult>().ToList();
            }

            var result = new DropDownPagedResult
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
