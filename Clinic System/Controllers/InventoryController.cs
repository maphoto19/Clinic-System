using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clinic_System.Models;

namespace Clinic_System.Controllers
{
    public class InventoryController : Controller
    {
        private ClinicSysDbEntities db = new ClinicSysDbEntities();

        // GET: Inventory
        public ActionResult Index()
        {
            var inventories = db.Inventories.Include(i => i.AssetCategory).Include(i => i.AssetDescription).Include(i => i.AssetUsageType).Include(i => i.Campu).Include(i => i.Supplier);
            return View(inventories.ToList());
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.AssetCategories, "Id", "CategoryName");
            ViewBag.AssetDescriptionId = new SelectList(db.AssetDescriptions, "Id", "AssetDescr");
            ViewBag.UsageTypeId = new SelectList(db.AssetUsageTypes, "Id", "UsageTypeDescription");
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CampusId,CategoryId,AssetDescriptionId,Model,SerialNumber,Warranty,Guarantee,SupplierId,UsageTypeId,Location,AssetColor,Description,Price,Quantity")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventories.Add(inventory);
                db.SaveChanges();
                TempData["successMessage"] = $"Item successfully added";
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.AssetCategories, "Id", "CategoryName", inventory.CategoryId);
            ViewBag.AssetDescriptionId = new SelectList(db.AssetDescriptions, "Id", "AssetDescr", inventory.AssetDescriptionId);
            ViewBag.UsageTypeId = new SelectList(db.AssetUsageTypes, "Id", "UsageTypeDescription", inventory.UsageTypeId);
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", inventory.CampusId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", inventory.SupplierId);
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.AssetCategories, "Id", "CategoryName", inventory.CategoryId);
            ViewBag.AssetDescriptionId = new SelectList(db.AssetDescriptions, "Id", "AssetDescr", inventory.AssetDescriptionId);
            ViewBag.UsageTypeId = new SelectList(db.AssetUsageTypes, "Id", "UsageTypeDescription", inventory.UsageTypeId);
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", inventory.CampusId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", inventory.SupplierId);
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CampusId,CategoryId,AssetDescriptionId,Model,AssignedToId,SerialNumber,Warranty,Guarantee,SupplierId,UsageTypeId,Location,AssetColor,Description,Price,Quantity")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.AssetCategories, "Id", "CategoryName", inventory.CategoryId);
            ViewBag.AssetDescriptionId = new SelectList(db.AssetDescriptions, "Id", "AssetDescr", inventory.AssetDescriptionId);
            ViewBag.UsageTypeId = new SelectList(db.AssetUsageTypes, "Id", "UsageTypeDescription", inventory.UsageTypeId);
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", inventory.CampusId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", inventory.SupplierId);
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveAsset(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
            db.SaveChanges();
            TempData["successMessage"] = $"Item successfully deleted";
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
