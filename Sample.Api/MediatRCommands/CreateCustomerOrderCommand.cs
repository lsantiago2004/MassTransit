using MediatR;
using Sample.Api.MediatRQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Api.MediatRCommands
{
    public class CreateCustomerOrderCommand : IRequest<OrderResponse>
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        //public int CustomerId { get; set; }
        //public int ProductId { get; set; }
    }
}
