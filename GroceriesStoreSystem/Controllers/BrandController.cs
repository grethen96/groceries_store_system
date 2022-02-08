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
    public class BrandController : Controller
    {
        private MarketContext context;
       

        public BrandController(MarketContext con)
        {
            context = con;
        }
        public IActionResult BrandIndex()
        {
            return View("BrandIndex");
        }

        public IActionResult LoadData()
        {
            try
            {
                string sql = "Select * from Brands order by BrandId DESC";
                var result = context.Brands.FromSqlRaw(sql).ToList();
                var _data = new { data = result };
                return Json(_data);
            }
            catch (Exception e)
            {
                return new EmptyResult();
            }
        }

        public IActionResult CreateBrand(int id)
        {
            return PartialView("BrandsCreateForm", new Brands());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(int id, Brands record)
        {            
            if (id == 0)
            {
                try
                {
                    //record.CreatedBy = nameIs;                   
                   // record.Created = DateTime.Now;
                   
                    context.Add(record);
                    await context.SaveChangesAsync();
                    TempData["Create"] = "Success";
                }
                catch (Exception ex)
                {
                   // _logger.LogInformation(ex.Message + ex.StackTrace);
                }
            }           
            return Json(new { pageUrl = Url.Action("BrandIndex", "Brand") });
        }

        //[HttpGet]
        public async Task<IActionResult> EditBrand(int? id)
        {
            
            if (id != null)
            {
                Brands item = await context.Brands.FirstOrDefaultAsync(s => s.BrandId == id);
                if (item == null)
                {
                    return NotFound();
                }
                return PartialView("BrandsEditForm", item);
            }
            else
            {
                return RedirectToAction("BrandIndex", "Brand");
            }
        }
           

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBrand(int id, Brands record)
        {
            if (id != record.BrandId)
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
                    return RedirectToAction("BrandIndex", "Brand");
                }
                return Json(new { pageUrl = Url.Action("BrandIndex", "Brand") });
            }
            return View(context.Brands.Where(x => x.BrandId == id).FirstOrDefault<Brands>());
        }

        //delete
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    Brands record = context.Brands.Find(id);
                    context.Brands.Remove(record);
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
        public IActionResult SaveAllChanges(List<int> modifiedIds, List<Brands> newData)
        {
            try
            {
                if (modifiedIds.Count > 0)
                {
                    var uniqueIds = modifiedIds.Distinct();
                    Brands newRecord = null;
                    foreach (var id in uniqueIds)
                    {
                        var oldRecord = context.Brands.Where(c => c.BrandId == id).FirstOrDefault();
                        newRecord = newData.Where(c => c.BrandId == id).FirstOrDefault();
                                                
                        context.Entry<Brands>(oldRecord).State = EntityState.Detached;

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

        //public JsonResult GetDropDownLists(string domain)
        //{
        //    //List<string> emailList = CRContext.AssignedEmails.Select(x => x.Email).Distinct().ToList();
        //    //emailList.Add("");
        //    //List<string> reportsList = OneBeContext.VBasicReport.Where(w => w.ParentMetricsName != null).OrderBy(x => x.ReportNo).Select(x => x.ReportNo + " - " + x.ReportName).Distinct().ToList();
        //    //List<string> domainOwnersList = lists.GetDomainOwnersList();
        //    //List<string> statusCrNrr = lists.StatusList();

           
        //    //var result = new { emails = emailList, reports = reportsList, domains = domainOwnersList, requestTypes = requestTypeList, statusList = statusCrNrr };
        //    return Json(null);//result
        //}


        //public void PopulateDropDownLists(object selectedItem = null)
        //{         
        //    List<SelectListItem> categoriesList = GetProductCategories();
        //    ViewBag.categories = new SelectList(categoriesList.OrderBy(x => x.Value), "Text", "Value", selectedItem);
        //    List<SelectListItem> brandsList = GetBrandNames();
        //    ViewBag.brands = new SelectList(brandsList.OrderBy(x => x.Value), "Text", "Value", selectedItem);

        //    //SelectListItem[] toolList = lists.GetTools();
        //    //ViewBag.tools = new SelectList(toolList, "Text", "Value", selectedItem);           
        //}
               
      
    }
}
