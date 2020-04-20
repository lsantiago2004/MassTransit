using MediatR;
using Sample.Api.MediatRCommands;
using Sample.Api.MediatRQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Api.MediatRHandlers
{
    public class CreateCustomerOrderHandler : IRequestHandler<CreateCustomerOrderCommand, OrderResponse>
    {
        //private readonly IOrdersRepository _ordersRepository;
        //private readonly IMapper _mapper;

        //public GetAllOrdersHandler(IOrderRepository ordersRepository, IMapper mapper)
        //{
        //_ordersRepository = _ordersRepository;
        //_mapper = mapper;
        //}

        public async Task<OrderResponse> Handle(CreateCustomerOrderCommand request, CancellationToken cancellationToken)
        {
            return new OrderResponse()
            {

                OrderId = request.OrderId,
                CustomerName = request.CustomerName,
                CustomerNumber = request.CustomerNumber

            };
        }

    }
}
