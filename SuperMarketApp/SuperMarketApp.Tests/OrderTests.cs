using System;
using System.Linq;
using Xunit;
using NSubstitute;
using SuperMarketApp.Interfaces;
using SuperMarketApp.Implementation;

namespace SuperMarketApp.Tests
{
    public class OrderTests
    {
        private readonly IVolumePricingRulesRepository _pricingRulesRepository;
        private readonly IPricingStrategyFactory _pricingStrategyFactory;
        private readonly IPricingStrategy _pricingStrategy;

        public OrderTests()
        {
            _pricingRulesRepository = Substitute.For<IVolumePricingRulesRepository>();
            _pricingStrategy = Substitute.For<IPricingStrategy>();
            _pricingStrategyFactory = new PricingStrategyFactory(_pricingRulesRepository);
        }

        [Fact]
        public void T01_AddOrderItem_SingleProduct_NoDiscount()
        {
            //Arrange
            var productC = new Product(Guid.NewGuid(), "C", 0.70m);

            _pricingRulesRepository.GetByProductId(productC.ProductId).Returns(r => null);

            var order = new CheckOutOrder(Guid.NewGuid(), _pricingStrategyFactory);


            //Act
            order.AddOrderItem(productC, 3);

            decimal expectedTotalPrice = 2.10m;


            //Assert
            Assert.Equal(expectedTotalPrice, order.GetTotalPrice());
        }

        [Fact]
        public void T02_AddOrderItem_MultipleProduct_NoDiscount()
        {
            //Arrange
            var productC = new Product(Guid.NewGuid(), "C", 0.70m);
            _pricingRulesRepository.GetByProductId(productC.ProductId).Returns(r => null);

            var productD = new Product(Guid.NewGuid(), "D", 0.20m);
            _pricingRulesRepository.GetByProductId(productD.ProductId).Returns(r => null);

            var order = new CheckOutOrder(Guid.NewGuid(), _pricingStrategyFactory);


            //Act
            order.AddOrderItem(productC, 5);
            order.AddOrderItem(productD, 2);

            decimal expectedTotalPrice = 3.90m;


            //Assert
            Assert.Equal(expectedTotalPrice, order.GetTotalPrice());
        }

        [Fact]
        public void T03_AddOrderItem_SingleProduct_ShouldApplyDiscount()
        {
            //Arrange
            var productA = new Product(Guid.NewGuid(), "A", 0.50m);
            var pricingRuleForC = new VolumePricingRule(Guid.NewGuid(), productA.ProductId, 3, 1.30m);
            _pricingRulesRepository.GetByProductId(productA.ProductId).Returns(pricingRuleForC);

            var order = new CheckOutOrder(Guid.NewGuid(), _pricingStrategyFactory);


            //Act
            order.AddOrderItem(productA, 10);

            decimal expectedTotalPrice = 4.40m;


            //Assert
            Assert.Equal(expectedTotalPrice, order.GetTotalPrice());
        }

        [Fact]
        public void T04_AddOrderItem_MultipleProduct_MultipleUnits_ShouldApplyDiscount()
        {
            //Arrange
            var productA = new Product(Guid.NewGuid(), "A", 0.50m);
            var pricingRuleForA = new VolumePricingRule(Guid.NewGuid(), productA.ProductId, 3, 1.30m);
            _pricingRulesRepository.GetByProductId(productA.ProductId).Returns(pricingRuleForA);

            var productB = new Product(Guid.NewGuid(), "B", 0.30m);
            var pricingRuleForB = new VolumePricingRule(Guid.NewGuid(), productB.ProductId, 2, 0.45m);
            _pricingRulesRepository.GetByProductId(productB.ProductId).Returns(pricingRuleForB);

            var productC = new Product(Guid.NewGuid(), "C", 0.70m);
            _pricingRulesRepository.GetByProductId(productC.ProductId).Returns(r => null);

            var productD = new Product(Guid.NewGuid(), "D", 0.20m);
            _pricingRulesRepository.GetByProductId(productD.ProductId).Returns(r => null);

            var order = new CheckOutOrder(Guid.NewGuid(), _pricingStrategyFactory);


            //Act
            order.AddOrderItem(productA);
            order.AddOrderItem(productB);
            order.AddOrderItem(productC);
            order.AddOrderItem(productD);
            order.AddOrderItem(productA);
            order.AddOrderItem(productB);
            order.AddOrderItem(productA);

            decimal expectedTotalPrice = 2.65m;


            //Assert
            Assert.Equal(expectedTotalPrice, order.GetTotalPrice());
        }

        [Fact]
        public void T05_AddOrderItem_MultipleProduct_SingleUnit_ShouldNotApplyDiscount()
        {
            //Arrange
            var productA = new Product(Guid.NewGuid(), "A", 0.50m);
            var pricingRuleForA = new VolumePricingRule(Guid.NewGuid(), productA.ProductId, 3, 1.30m);
            _pricingRulesRepository.GetByProductId(productA.ProductId).Returns(pricingRuleForA);

            var productB = new Product(Guid.NewGuid(), "B", 0.30m);
            var pricingRuleForB = new VolumePricingRule(Guid.NewGuid(), productB.ProductId, 2, 0.45m);
            _pricingRulesRepository.GetByProductId(productB.ProductId).Returns(pricingRuleForB);

            var productC = new Product(Guid.NewGuid(), "C", 0.70m);
            _pricingRulesRepository.GetByProductId(productC.ProductId).Returns(r => null);

            var productD = new Product(Guid.NewGuid(), "D", 0.20m);
            _pricingRulesRepository.GetByProductId(productD.ProductId).Returns(r => null);

            var order = new CheckOutOrder(Guid.NewGuid(), _pricingStrategyFactory);


            //Act
            order.AddOrderItem(productA);
            order.AddOrderItem(productB);
            order.AddOrderItem(productC);
            order.AddOrderItem(productD);

            decimal expectedTotalPrice = 1.70m;


            //Assert
            Assert.Equal(expectedTotalPrice, order.GetTotalPrice());
        }

        [Fact]
        public void T06_ProductOrder_Set_Invalid_Units()
        {
            //Arrange
            const int unitPrice = 12;
            const int units = -1;


            //Act - Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new ProductOrder(Guid.NewGuid(), Guid.NewGuid(), unitPrice, _pricingStrategy, units));
        }

        [Fact]
        public void T07_ProductOrder_Add_Invalid_Units()
        {
            //Arrange
            const int unitPrice = 12;
            var productOrder = new ProductOrder(Guid.NewGuid(), Guid.NewGuid(), unitPrice, _pricingStrategy);


            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => productOrder.AddUnits(-1));
        }

        [Fact]
        public void T08_ProductOrder_AddUnits_UpdatesUnits()
        {
            //Arrange
            const int unitPrice = 12;
            var productOrder = new ProductOrder(Guid.NewGuid(), Guid.NewGuid(), unitPrice, _pricingStrategy);


            //Act
            productOrder.AddUnits(1);

            const int expectedUnits = 2;


            //Assert
            Assert.Equal(expectedUnits, productOrder.GetUnits());
        }

        [Fact]
        public void T09_CheckOutOrder_AddOrderItem_ExistingOrderItem_UpdatesUnits()
        {
            //Arrange
            const int unitPrice = 12;
            var product = new Product(Guid.NewGuid(), "product", unitPrice);
            var order = new CheckOutOrder(Guid.NewGuid(), _pricingStrategyFactory);

            //Act
            order.AddOrderItem(product);
            order.AddOrderItem(product);

            const int orderItems = 1;
            const int expectedUnits = 2;

            //Assert
            Assert.Equal(orderItems, order.OrderItems.Count);
            Assert.Equal(expectedUnits, order.OrderItems.First().GetUnits());
        }
    }
}
