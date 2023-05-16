using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mozzhhevelnik.Models
{
    public class Cart
    {
        [Key]
        public int id { get; set; }
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(disks dissk, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.disk.id == dissk.id)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    disk = dissk,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(disks dissk)
        {
            lineCollection.RemoveAll(l => l.disk.id == dissk.id);
        }

        public double ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.disk.price * e.Quantity);

        }
        public double TotalAmount()
        {
            if (lineCollection==null)
               return 0;
            else 
                return lineCollection.Sum(e => e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public disks disk { get; set; }
        public int Quantity { get; set; }
    }

}