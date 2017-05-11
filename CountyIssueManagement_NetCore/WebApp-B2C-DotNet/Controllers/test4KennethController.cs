using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp_OpenIDConnect_DotNet_B2C.DatabaseContext;
using WebApp_OpenIDConnect_DotNet_B2C.Models;

namespace WebApp_OpenIDConnect_DotNet_B2C.Controllers
{
    [Authorize(Roles = "officer")]
    public class test4KennethController : Controller
    {
        private IssueManagementDbContext db = new IssueManagementDbContext();

        // GET: test4Kenneth
        public ActionResult Index()
        {
            return View(db.test4Kenneth.ToList());
        }

        // GET: test4Kenneth/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test4Kenneth test4Kenneth = db.test4Kenneth.Find(id);
            if (test4Kenneth == null)
            {
                return HttpNotFound();
            }
            return View(test4Kenneth);
        }

        // GET: test4Kenneth/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: test4Kenneth/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] test4Kenneth test4Kenneth)
        {
            if (ModelState.IsValid)
            {
                db.test4Kenneth.Add(test4Kenneth);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(test4Kenneth);
        }

        // GET: test4Kenneth/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test4Kenneth test4Kenneth = db.test4Kenneth.Find(id);
            if (test4Kenneth == null)
            {
                return HttpNotFound();
            }
            return View(test4Kenneth);
        }

        // POST: test4Kenneth/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] test4Kenneth test4Kenneth)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test4Kenneth).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(test4Kenneth);
        }

        // GET: test4Kenneth/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test4Kenneth test4Kenneth = db.test4Kenneth.Find(id);
            if (test4Kenneth == null)
            {
                return HttpNotFound();
            }
            return View(test4Kenneth);
        }

        // POST: test4Kenneth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            test4Kenneth test4Kenneth = db.test4Kenneth.Find(id);
            db.test4Kenneth.Remove(test4Kenneth);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
