using CbData.Interview.Abstraction;
using CbData.Interview.Abstraction.Model;
using NUnit.Framework;

namespace CbData.Interview.Test
{
    public abstract class ModelDataConnectorTest
    {
        public abstract IModelDataConnector SetupConnector();

        private readonly IModelDataConnector _connector;

        public ModelDataConnectorTest()
        {
            _connector = SetupConnector();
        }

        [Test]
        public void InsertOrderTest()
        {
            var order = new Order
            {
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 1
            };

            _connector.Insert(order);
            _connector.Commit();
            Assert.That(order.Id, Is.GreaterThan(0));

            var receivedOrder = _connector.Query<Order>().SingleOrDefault(o => o.Id == order.Id);
            Assert.That(receivedOrder, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(receivedOrder.ProductId, Is.EqualTo(order.ProductId));
                Assert.That(receivedOrder.Quantity, Is.EqualTo(order.Quantity));
                Assert.That(receivedOrder.IsAnnounced, Is.False);
            });
        }

        [Test]
        public void UpdateOrderTest()
        {
            var order = new Order
            {
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 1
            };

            _connector.Insert(order);
            _connector.Commit();

            var receivedOrder = _connector.Query<Order>().SingleOrDefault(o => o.ProductId == order.ProductId);
            Assert.That(receivedOrder, Is.Not.Null);
            Assert.That(receivedOrder.IsAnnounced, Is.False);

            order.IsAnnounced = true;

            _connector.Update(order);
            _connector.Commit();

            receivedOrder = _connector.Query<Order>().SingleOrDefault(o => o.ProductId == order.ProductId);
            Assert.That(receivedOrder.IsAnnounced, Is.True);
        }

        [Test]
        public void DeleteOrderTest()
        {
            var order = new Order
            {
                ProductId = Guid.NewGuid().ToString(),
                Quantity = 1
            };

            _connector.Insert(order);
            _connector.Commit();

            var receivedOrder = _connector.Query<Order>().SingleOrDefault(o => o.ProductId == order.ProductId);
            Assert.That(receivedOrder, Is.Not.Null);

            _connector.Delete(order);
            _connector.Commit();

            receivedOrder = _connector.Query<Order>().SingleOrDefault(o => o.ProductId == order.ProductId);
            Assert.That(receivedOrder, Is.Null);
        }
    }
}
