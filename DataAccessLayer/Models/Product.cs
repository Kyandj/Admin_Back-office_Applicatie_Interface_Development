using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ArticleNumber { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public string ImageUrl { get; set; } 
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}
