using System;

namespace SuperMarketApp.Implementation
{
    public class Product
    {
        public Guid ProductId { get; private set; }
        public string Name { get; private set; }
        public decimal UnitPrice { get; private set; }

        public Product(Guid productId, string name, decimal unitPrice)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException(nameof(name)); }
            if (unitPrice < 0) { throw new ArgumentOutOfRangeException(nameof(unitPrice)); }

            ProductId = productId;
            Name = name;
            UnitPrice = unitPrice;
        }
    }
}
