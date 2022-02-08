
using GroceriesStoreSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceriesStoreSystem.Services
{
    public class DropdownLists
    {
        public MarketContext context; //= new MarketContext();

        public DropdownLists(MarketContext con)
        {
            context = con;
        }
        public SelectListItem[] DistributionMethodList()
        {
            return new SelectListItem[]
                        {
                new SelectListItem() { Text = "Email", Value = "Email"},
                new SelectListItem() { Text = "OneBE", Value = "OneBE" },
                new SelectListItem() { Text = "SharePoint", Value = "SharePoint"},
                new SelectListItem() { Text = "On-demand", Value = "On-demand" },
                        };
        }
        /// <summary>
        /// This method is populating the drop-down lists for categories in each form.
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetProductCategories()
        {
            List<SelectListItem> categoriesList = new List<SelectListItem>();
            var list = context.ProductCategory.Where(p => p.IsActive == true).Select(x => new { x.CategoryId,x.CategoryName }).Distinct().ToList();
            foreach (var item in list)
            {
                categoriesList.Add(new SelectListItem() { Text = item.CategoryName, Value = item.CategoryName });
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
            List<SelectListItem>  brandsList = new List<SelectListItem>();
            var list = context.Brands.Where(p => p.IsActive == true).Select(x => new { x.BrandId, x.BrandName }).Distinct().ToList();
            foreach (var item in list)
            {
                brandsList.Add(new SelectListItem() { Text = item.BrandName, Value = item.BrandName });
            }
            return brandsList;
        }
    }
}
