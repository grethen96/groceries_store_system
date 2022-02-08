using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroceriesStoreSystem.Models
{
    public class ProductCategory
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Groceries> Groceries { get; set; }
    }
}
