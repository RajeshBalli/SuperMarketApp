using System;
using SuperMarketApp.Interfaces;

namespace SuperMarketApp.Implementation
{
    public class PricingStrategyFactory : IPricingStrategyFactory
    {
        private readonly IVolumePricingRulesRepository _volumePricingRulesRepository;

        public PricingStrategyFactory(IVolumePricingRulesRepository volumePricingRulesRepository)
        {
            _volumePricingRulesRepository = volumePricingRulesRepository;
        }

        public IPricingStrategy Create(Guid productId)
        {
            var volumeRule = _volumePricingRulesRepository.GetByProductId(productId);

            if (volumeRule != null)
            {
                return new VolumePricingStrategy(volumeRule.Units,
                    volumeRule.Price);
            }

            return new RegularPricingStrategy();
        }
    }
}
