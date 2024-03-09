using CbData.Interview.Abstraction.Model;
using System.Text.Json.Serialization;

namespace CbData.Interview.Web.Api.Model
{
    /// <summary>
    /// Represents a new <see cref="Order"/> input.
    /// </summary>
    public class OrderInput
    {
        [JsonPropertyName("productId")]
        public required string ProductId { get; set; }

        /// <summary>
        /// Gets or sets a number of product quantity.
        /// </summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets a new <see cref="Order"/> object.
        /// </summary>
        /// <returns></returns>
        internal Order ToOrder()
        {
            return new Order
            { 
                ProductId = ProductId,
                Quantity = Quantity
            };
        }
    }
}
