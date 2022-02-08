using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceriesStoreSystem.Models;
using GroceriesStoreSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GroceriesStoreSystem.Controllers
{
    public class CategoryController : Controller
    {
        private MarketContext context;


        public CategoryController(MarketContext con)
        {
            context = con;
        }
        public IActionResult CategoryIndex()
        {
            return View("CategoryIndex");
        }

        public IActionResult LoadData()
        {
            try
            {
                string sql = "select * from ProductCategory order by CategoryId desc";
                var result = context.ProductCategory.FromSqlRaw(sql).ToList();
                var _data = new { data = result };
                return Json(_data);
            }
            catch (Exception e)
            {
                return new EmptyResult();
            }
        }

        public IActionResult CreateCategory(int id)
        {
            return PartialView("CategoryCreateForm", new ProductCategory());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(int id, ProductCategory record)
        {
            if (id == 0)
            {
                try
                {
                    context.Add(record);
                    await context.SaveChangesAsync();
                    TempData["Create"] = "Success";
                }
                catch (Exception ex)
                {
                    // _logger.LogInformation(ex.Message + ex.StackTrace);
                }
            }
            return Json(new { pageUrl = Url.Action("CategoryIndex", "Category") });
        }

        //[HttpGet]
        public async Task<IActionResult> EditCategory(int? id)
        {

            if (id != null)
            {
                ProductCategory item = await context.ProductCategory.FirstOrDefaultAsync(s => s.CategoryId == id);
                
                if (item == null)
                {
                    return NotFound();
                }
                return PartialView("CategoryEditForm", item);
            }
            else
            {
                return RedirectToAction("CategoryIndex", "Category");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, ProductCategory record)
        {
            if (id != record.CategoryId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(record);
                    await context.SaveChangesAsync();
                    TempData["Edit"] = "Success";

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "contact your system administrator.");
                    //  _logger.LogInformation(ex.Message + ex.StackTrace);
                    return RedirectToAction("CategoryIndex", "Category");
                }
                return Json(new { pageUrl = Url.Action("CategoryIndex", "Category") });
            }
         
            return View(context.ProductCategory.Where(x => x.CategoryId == id).FirstOrDefault<ProductCategory>());
        }

        //delete
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    ProductCategory record = context.ProductCategory.Find(id);
                    context.ProductCategory.Remove(record);
                    context.SaveChanges();
                    return Json(new { data = true });
                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (Exception)
            {
                return Json(data: false);
            }
        }

        [HttpPost]
        public IActionResult SaveAllChanges(List<int> modifiedIds, List<ProductCategory> newData)
        {
            try
            {
                if (modifiedIds.Count > 0)
                {
                    var uniqueIds = modifiedIds.Distinct();
                    ProductCategory newRecord = null;
                    foreach (var id in uniqueIds)
                    {
                        var oldRecord = context.ProductCategory.Where(c => c.CategoryId == id).FirstOrDefault();
                        newRecord = newData.Where(c => c.CategoryId == id).FirstOrDefault();

                        context.Entry<ProductCategory>(oldRecord).State = EntityState.Detached;

                        context.Update(newRecord);
                        context.SaveChanges();
                    }
                    return Json(new { data = "success" });
                }
                else
                {
                    return Json(new { data = "fail" });
                }

            }
            catch (Exception ex)
            {
                // _logger.LogInformation(ex.Message + ex.StackTrace);
                return Json(new { data = "fail" });
                // throw;
            }
        }

    }
}
