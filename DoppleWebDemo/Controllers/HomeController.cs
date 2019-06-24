﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoppleWebDemo.Models;
using DoppleWebDemo.Controllers.Helpers;
using GraphSimilarityByMatching;
using System.Diagnostics;

namespace DoppleWebDemo.Controllers
{
    public class FunctionComparisonsController : Controller
    {
        private FunctionComparisonDBContext db = new FunctionComparisonDBContext();

        // GET: FunctionComparisons
        public ActionResult Index()
        {
            return View(db.FunctionComparisons.ToList());
        }

        // GET: FunctionComparisons/Details/5
        public ActionResult Intro()
        {
            return View();
        }

        public ActionResult Background()
        {
            return View();
        }

        // GET: FunctionComparisons/Create
        public ActionResult Compare()
        {
            FunctionComparison functionComparison = new FunctionComparison();
            functionComparison.FirstFunctionCode = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/function_text_placeholder1.txt"));
            functionComparison.SecondFunctionCode = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/function_text_placeholder2.txt"));

            return View(functionComparison);
        }

        // POST: FunctionComparisons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compare([Bind(Include = "Index,FirstFunctionCode,SecondFunctionCode")] FunctionComparison functionComparison)
        {
            //return View("Result");
            if (ModelState.IsValid)
            {
                try
                {
                    var blah = db.FunctionComparisons.Add(functionComparison);
                    functionComparison.CalculateScores();
                    var id = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }

                return View("Result", functionComparison);
            }

            return View(functionComparison);
        }

        // GET: FunctionComparisons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionComparison functionComparison = db.FunctionComparisons.Find(id);
            if (functionComparison == null)
            {
                return HttpNotFound();
            }
            return View(functionComparison);
        }

        // POST: FunctionComparisons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstFunctionCode,SecondFunctionCode")] FunctionComparison functionComparison)
        {
            if (ModelState.IsValid)
            {
                db.Entry(functionComparison).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(functionComparison);
        }

        public ActionResult Result(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionComparison functionComparison = db.FunctionComparisons.Find(id);
            if (functionComparison == null)
            {
                return HttpNotFound();
            }
            return View(functionComparison);
        }

        // GET: FunctionComparisons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FunctionComparison functionComparison = db.FunctionComparisons.Find(id);
            if (functionComparison == null)
            {
                return HttpNotFound();
            }
            return View(functionComparison);
        }


        // GET: FunctionComparisons/Edit/5
        public ActionResult Feedback()
        {
            return View();
        }


        // POST: FunctionComparisons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FunctionComparison functionComparison = db.FunctionComparisons.Find(id);
            db.FunctionComparisons.Remove(functionComparison);
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
