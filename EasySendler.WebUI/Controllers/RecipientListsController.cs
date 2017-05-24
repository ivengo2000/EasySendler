using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuisenessLogicLayer;
using EasySendler.Models.BusinessLogic;

namespace EasySendler.Controllers
{
    public class RecipientListsController : Controller
    {
        private readonly MySmtpEntities _db = new MySmtpEntities();

        public RecipientListsController()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RecipientList, RecipientListViewModel>());
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
        public ActionResult GetRecipientListForDropDown(string searchTerm, int pageSize, int pageNum)
        {
            ////Get the paged results and the total count of the results for this query. 
            //AttendeeRepository ar = new AttendeeRepository();
            //List<Attendee> attendees = ar.GetAttendees(searchTerm, pageSize, pageNum);
            //int attendeeCount = ar.GetAttendeesCount(searchTerm, pageSize, pageNum);

            ////Translate the attendees into a format the select2 dropdown expects
           // Select2PagedResult pagedAttendees = AttendeesToSelect2Format(attendees, attendeeCount);
            Select2PagedResult pagedAttendees = new Select2PagedResult();
            var resultList = new List<Select2Result>
            {
                new Select2Result() {id = "1", text = "test1"},
                new Select2Result() {id = "2", text = "test2"},
                new Select2Result() {id = "3", text = "test3"}
            };

            pagedAttendees.Results = resultList;
            pagedAttendees.Total = 3;

            return new JsonResult
            {
                Data = pagedAttendees,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public class Select2PagedResult
        {
            public int Total { get; set; }
            public List<Select2Result> Results { get; set; }
        }

        //[Serializable]
        public class Select2Result
        {
            //[DisplayName("id")]
            public string id { get; set; }
            //[DisplayName("text")]
            public string text { get; set; }
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
