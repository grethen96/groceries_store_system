using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroceriesStoreSystem.Models
{
    public class Groceries
    {
        [Key]
        public int ProductId { get; set; }
        public string Code { get; set; }
        //public string Category { get; set; }
        public double? Price { get; set; }
        public string BrandName { get; set; }
        public double? Quantity { get; set; }
        public DateTime? ValidDate { get; set; }

        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Supplier { get; set; }

        public string CategoryNameView { get; set; }

        public bool NeedToAddQuantity { get; set; }

        [Column("CategoryIdProduct")]
        public int CategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }
    }
}
