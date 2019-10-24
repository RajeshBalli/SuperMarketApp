using System;
using SuperMarketApp.Interfaces;

namespace SuperMarketApp.Implementation
{
    public class ProductOrder : IProductOrder
    {
        public Guid ProductOrderId { get; }
        public Guid ProductId { get; }

        private readonly IPricingStrategy _pricingStrategy;

        private readonly decimal _unitPrice;
        private int _units;

        public ProductOrder(Guid productOrderId, Guid productId, decimal unitPrice, IPricingStrategy pricingStrategy, int units = 1)
        {
            if (units < 1) { throw new ArgumentOutOfRangeException(nameof(units)); }
            if (unitPrice < 0) { throw new ArgumentOutOfRangeException(nameof(unitPrice)); }

            _pricingStrategy = pricingStrategy ?? throw new ArgumentNullException(nameof(pricingStrategy));

            ProductOrderId = productOrderId;
            ProductId = productId;

            _unitPrice = unitPrice;
            _units = units;
        }

        /// <summary>
        /// Gets the unit number of the Product.
        /// </summary>
        /// <returns>A <see cref="int"/>.</returns>
        public int GetUnits()
        {
            return _units;
        }

        /// <summary>
        /// Gets the unit price of the Product.
        /// </summary>
        /// <returns>A <see cref="decimal"/> object</returns>
        public decimal GetUnitPrice()
        {
            return _unitPrice;
        }

        /// <summary>
        /// Gets the total price based on the number of units of the Product.
        /// </summary>
        /// <returns>A <see cref="decimal"/> object</returns>
        public decimal GetTotalPrice()
        {
            return _pricingStrategy.GetTotal(this);
        }

        /// <summary>
        /// Adds to the units of the existing Product.
        /// </summary>
        /// <param name="units"></param>
        public void AddUnits(int units)
        {
            if (units < 1) { throw new ArgumentOutOfRangeException(nameof(units)); }

            _units += units;
        }
    }
}
