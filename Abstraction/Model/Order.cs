using System.Runtime.Serialization;

namespace CbData.Interview.Abstraction.Model
{
    /// <summary>
    /// Represents an order.
    /// </summary>
    [DataContract]
    public class Order : IEntity
    {       
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a product Id.
        /// </summary>
        [DataMember(Name = "ProductId")]
        public required string ProductId { get; set; }

        /// <summary>
        /// Gets or sets a number of product quantity.
        /// </summary>
        [DataMember(Name = "Quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets a value that determine if the order was processed or not.
        /// </summary>
        public bool IsAnnounced { get; set; }
    }
}
