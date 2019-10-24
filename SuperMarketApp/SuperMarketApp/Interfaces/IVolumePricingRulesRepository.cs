using System;
using SuperMarketApp.Implementation;

namespace SuperMarketApp.Interfaces
{
    public interface IVolumePricingRulesRepository
    {
        VolumePricingRule GetByProductId(Guid productId);
    }
}
