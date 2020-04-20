using MediatR;
using Sample.Api.MediatRQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Api.MediatRHandlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse>
    {
        //private readonly IOrdersRepository _ordersRepository;
        //private readonly IMapper _mapper;

        //public GetOrderByIdHandler(IOrderRepository ordersRepository, IMapper mapper)
        //{
        //_ordersRepository = _ordersRepository;
        //_mapper = mapper;
        //}

        public GetOrderByIdHandler()
        {

        }
        public async Task<OrderResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            return new OrderResponse()
            {
                           
                OrderId = request.Id,
                CustomerName = "Luis",
                CustomerNumber = "1234"

            };
        
        }
    }
}
