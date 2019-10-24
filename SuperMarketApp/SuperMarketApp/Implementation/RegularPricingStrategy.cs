using SuperMarketApp.Interfaces;

namespace SuperMarketApp.Implementation
{
    public class RegularPricingStrategy : IPricingStrategy
    {
        public virtual decimal GetTotal(IProductOrder item)
        {
            int units = item.GetUnits();
            decimal unitPrice = item.GetUnitPrice();

            return units * unitPrice;
        }
    }
}
