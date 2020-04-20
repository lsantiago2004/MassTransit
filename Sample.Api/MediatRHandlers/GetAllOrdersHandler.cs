using MediatR;
using Sample.Api.MediatRQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Api.MediatRHandlers
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderResponse>>
    {
        //private readonly IOrdersRepository _ordersRepository;
        //private readonly IMapper _mapper;

        //public GetAllOrdersHandler(IOrderRepository ordersRepository, IMapper mapper)
        //{
        //_ordersRepository = _ordersRepository;
        //_mapper = mapper;
        //}
        public GetAllOrdersHandler()
        {

        }
        public async Task<List<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orderList = new List<OrderResponse>();
            orderList.Add(new OrderResponse()
            {
                OrderId = 5,
                CustomerName = "Luis",
                CustomerNumber = "1234"

            });
            return orderList.ToList();

            //var orders = await _ordersRepository.GetOrdersAsync();
            //return _mapper.MapOrderDtosToOrderResponse(orders);
        }
    }
}
