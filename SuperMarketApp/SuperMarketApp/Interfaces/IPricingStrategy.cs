
namespace SuperMarketApp.Interfaces
{
    public interface IPricingStrategy
    {
        decimal GetTotal(IProductOrder product);
    }
}
