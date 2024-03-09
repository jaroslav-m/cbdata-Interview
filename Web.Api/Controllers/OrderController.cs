using CbData.Interview.Abstraction;
using CbData.Interview.Web.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace CbData.Interview.Web.Api.Controllers
{
    [ApiController]
    [Route("api/v1/order")]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostOrder(
            [FromServices] IModelDataConnector modelDataConnector,
            IEnumerable<OrderInput> orders)
        {
            foreach (var order in orders)
                modelDataConnector.Insert(order.ToOrder());

            modelDataConnector.Commit();

            return Ok();
        }
    }
}
