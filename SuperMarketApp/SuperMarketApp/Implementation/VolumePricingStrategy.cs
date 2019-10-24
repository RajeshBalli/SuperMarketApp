using System;
using SuperMarketApp.Interfaces;

namespace SuperMarketApp.Implementation
{
    public class VolumePricingStrategy : RegularPricingStrategy
    {
        private readonly decimal _volumePrice;
        private readonly int _volumeThreshold;

        public VolumePricingStrategy(int volumeThreshold, decimal volumePrice)
        {
            if (volumeThreshold < 1) { throw new ArgumentOutOfRangeException(nameof(volumeThreshold)); }

            _volumePrice = volumePrice;
            _volumeThreshold = volumeThreshold;
        }

        public override decimal GetTotal(IProductOrder item)
        {
            var regularPrice = base.GetTotal(item);
            decimal volumeDiscount = 0;
            int units = item.GetUnits();
            decimal unitPrice = item.GetUnitPrice();

            if (units >= _volumeThreshold)
            {
                volumeDiscount = _volumeThreshold * unitPrice - _volumePrice;

                int multiplyDiscount = units / _volumeThreshold;

                //Handle if volumeThreshold is 3 and units are 6 then the discount should be applied twice
                if (multiplyDiscount > 1)
                    volumeDiscount *= multiplyDiscount;
            }

            return regularPrice - volumeDiscount;
        }
    }
}
