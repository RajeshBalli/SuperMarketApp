using System;

namespace SuperMarketApp.Interfaces
{
    public interface IPricingStrategyFactory
    {
        IPricingStrategy Create(Guid productId);
    }
}
