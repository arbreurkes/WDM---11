using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class OrderItem
    {
        public Stock Item { get; set; } //FK ?
        public int Quantity { get; set; }

        public void IncQuantity()
        {
            Quantity += 1;
        }

        public void DecQuantity()
        {
            Quantity -= 1;

            if (Quantity < 1)
            {
                throw new InvalidQuantityException();
            }
        }
    }
}
