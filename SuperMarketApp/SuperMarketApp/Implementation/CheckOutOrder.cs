using System;
using System.Collections.Generic;
using System.Linq;
using SuperMarketApp.Interfaces;

namespace SuperMarketApp.Implementation
{
    public class CheckOutOrder
    {
        private readonly Guid _orderId;
        private readonly List<ProductOrder> _orderItems;
        private readonly IPricingStrategyFactory _pricingStrategyFactory;

        public IReadOnlyCollection<ProductOrder> OrderItems => _orderItems;

        public CheckOutOrder(Guid orderId, IPricingStrategyFactory pricingStrategyFactory)
        {
            _orderId = orderId;
            _pricingStrategyFactory = pricingStrategyFactory;
            _orderItems = new List<ProductOrder>();
        }

        /// <summary>
        /// Adds a Product into the OrderItem with one or several units.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="units"></param>
        public void AddOrderItem(Product product, int units = 1)
        {
            var existingOrderForProduct = _orderItems.SingleOrDefault(o => o.ProductId == product.ProductId);

            if (existingOrderForProduct != null)
            {
                existingOrderForProduct.AddUnits(units);
            }
            else
            {
                var pricingStrategy = _pricingStrategyFactory.Create(product.ProductId);
                var orderItem = new ProductOrder(Guid.NewGuid(), product.ProductId, product.UnitPrice, pricingStrategy, units);

                _orderItems.Add(orderItem);
            }
        }

        /// <summary>
        /// Gets the total price of the current order.
        /// </summary>
        /// <returns>A <see cref="decimal"/> object</returns>
        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (var orderItem in _orderItems)
            {
                totalPrice = totalPrice + orderItem.GetTotalPrice();
            }

            return totalPrice;
        }
    }
}
