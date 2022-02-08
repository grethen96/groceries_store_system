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
    public class HomeController : Controller
    {
        private MarketContext context;
       

        public HomeController(MarketContext con)
        {
            context = con;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadData()
        {
            try
            {
                string sql = "select * from Groceries gg inner join ProductCategory pc on gg.CategoryIdProduct = pc.CategoryId order by gg.ProductId DESC";
                var result = context.Groceries.FromSqlRaw(sql).ToList();
                var _data = new { data = result };
                return Json(_data);
            }
            catch (Exception e)
            {
                return new EmptyResult();
            }
        }

        public IActionResult CreateProducts(int id)
        {
            PopulateDropDownLists();           
            return PartialView("ProductCreateForm", new Groceries());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProducts(int id, Groceries record)
        {          
            if (id == 0)
            {
                try
                {
                    ProductCategory category = context.ProductCategory.Where(x => x.CategoryId == record.CategoryId).FirstOrDefault();
                    record.CategoryNameView = category.CategoryName;

                    if(record.Quantity < 0)
                    {
                        record.NeedToAddQuantity = true;
                    }
                    else
                    {
                        record.NeedToAddQuantity = false;
                    }

                    record.Created = DateTime.Now;
                   
                    context.Add(record);
                    await context.SaveChangesAsync();
                    TempData["Create"] = "Success";
                }
                catch (Exception ex)
                {
                   // _logger.LogInformation(ex.Message + ex.StackTrace);
                }
            }
            return Json(new { pageUrl = Url.Action("Index", "Home") });
        }

        
        public async Task<IActionResult> EditProducts(int? id)
        {
            if (id != null)
            {
                Groceries item = await context.Groceries.FirstOrDefaultAsync(s => s.ProductId == id);
                if (item == null)
                {
                    return NotFound();
                }
                string sqlFormattedDate = item.Created.HasValue
                             ? item.Created.Value.ToString("YY-MM-dd HH:mm:ss")
                             : "<not available>";
               
                PopulateDropDownLists();
                return PartialView("ProductEditForm", item);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
           

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducts(int id, Groceries record)
        {
            if (id != record.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ProductCategory category = context.ProductCategory.Where(x => x.CategoryId == record.CategoryId).FirstOrDefault();
                    record.CategoryNameView = category.CategoryName;

                    if (record.Quantity < 0)
                    {
                        record.NeedToAddQuantity = true;
                    }
                    else
                    {
                        record.NeedToAddQuantity = false;
                    }

                    context.Update(record);
                    await context.SaveChangesAsync();
                    TempData["Edit"] = "Success";

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "contact your system administrator.");
                  //  _logger.LogInformation(ex.Message + ex.StackTrace);
                    return RedirectToAction("Index", "Home");
                }
                PopulateDropDownLists();
                return Json(new { pageUrl = Url.Action("Index", "Home") });
            }
            PopulateDropDownLists();
            return View(context.Groceries.Where(x => x.ProductId == id).FirstOrDefault<Groceries>());
        }

        //delete
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    Groceries record = context.Groceries.Find(id);
                    context.Groceries.Remove(record);
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
        public IActionResult SaveAllChanges(List<int> modifiedIds, List<Groceries> newData)
        {
            try
            {
                if (modifiedIds.Count > 0)
                {
                    var uniqueIds = modifiedIds.Distinct();
                    Groceries newRecord = null;
                    foreach (var id in uniqueIds)
                    {
                        var oldRecord = context.Groceries.Where(c => c.ProductId == id).FirstOrDefault();
                        newRecord = newData.Where(c => c.ProductId == id).FirstOrDefault();
                                               
                        context.Entry<Groceries>(oldRecord).State = EntityState.Detached;

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

        public JsonResult GetDropDownLists(string domain)
        {
            List<string> brandsList = context.Brands.Select(x => x.BrandName).Distinct().ToList();
           
            //List<string> statusCrNrr = lists.StatusList();

            var result = new { brands = brandsList };
            return Json(result);
        }


        public void PopulateDropDownLists(object selectedItem = null)
        {         
            List<SelectListItem> categoriesList = GetProductCategories();
            ViewBag.categories = new SelectList(categoriesList.OrderBy(x => x.Value), "Text", "Value", selectedItem);
            List<SelectListItem> brandsList = GetBrandNames();
            ViewBag.brands = new SelectList(brandsList.OrderBy(x => x.Value), "Text", "Value", selectedItem);

            //SelectListItem[] toolList = lists.GetTools();
            //ViewBag.tools = new SelectList(toolList, "Text", "Value", selectedItem);           
        }

        public List<SelectListItem> GetProductCategories()
        {
            List<SelectListItem> categoriesList = new List<SelectListItem>();
            var list = context.ProductCategory.Where(p => p.IsActive == true).Select(x => new { x.CategoryId, x.CategoryName }).Distinct().ToList();
            foreach (var item in list)
            {
                categoriesList.Add(new SelectListItem() { Text = item.CategoryId.ToString(), Value = item.CategoryName });
            }
            return categoriesList;
        }
        /// <summary>
        /// This method is populating the drop-down lists in forms NRR and ARR.
        /// 'where metrics.ParentMetricsName != null' is there because there are NULL values in the column
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBrandNames()
        {
            List<SelectListItem> brandsList = new List<SelectListItem>();
            var list = context.Brands.Where(p => p.IsActive == true).Select(x => new { x.BrandId, x.BrandName }).Distinct().ToList();
            foreach (var item in list)
            {
                brandsList.Add(new SelectListItem() { Text = item.BrandName, Value = item.BrandName });
            }
            return brandsList;
        }
    }
}
