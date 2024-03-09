using CbData.Interview.Abstraction;
using CbData.Interview.Common;
using CbData.Interview.ModelDataConnector;
using Microsoft.EntityFrameworkCore;

namespace CbData.Interview.Test
{
    internal class DbModelDataConnectorTest : ModelDataConnectorTest
    {
        public override IModelDataConnector SetupConnector()
        {
            var options = new DbContextOptionsBuilder<ModelDataContext>()
                .UseInMemoryDatabase(databaseName: "ModelDataDb-test")
                .Options;
            var modelDataContext = new ModelDataContext(options);
            return new DbModelDataConnector(modelDataContext);
        }
    }
}
