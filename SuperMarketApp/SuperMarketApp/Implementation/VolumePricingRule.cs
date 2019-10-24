using System;

namespace SuperMarketApp.Implementation
{
    public class VolumePricingRule
    {
        public VolumePricingRule(Guid volumePricingRuleId, Guid productId, int units, decimal price)
        {
            if (units < 1) { throw new ArgumentOutOfRangeException(nameof(units)); }

            VolumePricingRuleId = volumePricingRuleId;
            ProductId = productId;
            Units = units;
            Price = price;
        }

        public Guid VolumePricingRuleId { get; }
        public Guid ProductId { get; }
        public decimal Price { get; }
        public int Units { get; }
    }
}
