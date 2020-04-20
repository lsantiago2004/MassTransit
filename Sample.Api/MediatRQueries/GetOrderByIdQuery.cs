using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Api.MediatRQueries
{
    public class GetOrderByIdQuery : IRequest<OrderResponse>
    {
        public int Id { get; }
        public GetOrderByIdQuery(int id)
        {
            Id = id;

        }
    }
}
