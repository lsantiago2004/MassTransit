using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Api.MediatRQueries
{
    public class GetAllOrdersQuery : IRequest<List<OrderResponse>>
    {

    }
}
