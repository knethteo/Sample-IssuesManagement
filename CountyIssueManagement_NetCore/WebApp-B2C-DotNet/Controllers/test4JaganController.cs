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
    public class test4JaganController : Controller
    {
        private IssueManagementDbContext db = new IssueManagementDbContext();

        // GET: test4Jagan
        public ActionResult Index()
        {
            return View(db.test4Jagan.ToList());
        }

        // GET: test4Jagan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test4Jagan test4Jagan = db.test4Jagan.Find(id);
            if (test4Jagan == null)
            {
                return HttpNotFound();
            }
            return View(test4Jagan);
        }

        // GET: test4Jagan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: test4Jagan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] test4Jagan test4Jagan)
        {
            if (ModelState.IsValid)
            {
                db.test4Jagan.Add(test4Jagan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(test4Jagan);
        }

        // GET: test4Jagan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test4Jagan test4Jagan = db.test4Jagan.Find(id);
            if (test4Jagan == null)
            {
                return HttpNotFound();
            }
            return View(test4Jagan);
        }

        // POST: test4Jagan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] test4Jagan test4Jagan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test4Jagan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(test4Jagan);
        }

        // GET: test4Jagan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            test4Jagan test4Jagan = db.test4Jagan.Find(id);
            if (test4Jagan == null)
            {
                return HttpNotFound();
            }
            return View(test4Jagan);
        }

        // POST: test4Jagan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            test4Jagan test4Jagan = db.test4Jagan.Find(id);
            db.test4Jagan.Remove(test4Jagan);
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
