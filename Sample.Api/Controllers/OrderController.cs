using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Api.MediatRCommands;
using Sample.Api.MediatRQueries;
using Sample.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
            private readonly ILogger<OrderController> _logger;

            //MassTransit RequestClient is injected by the constructor, the 
            //Controller is resolved using whatever ASP.NEt does to that.
            private readonly IRequestClient<SubmitOrder> _submitOrderRequestClient;

            //from MediatR and not MassTransit Mediator.
            private readonly MediatR.IMediator _mediator;

        public OrderController(ILogger<OrderController> logger, MediatR.IMediator mediator, IRequestClient<SubmitOrder> submitOrderRequestClient)
            {
                _logger = logger;
                _submitOrderRequestClient = submitOrderRequestClient;
                //MediatR
                _mediator = mediator;
            }

        //MassTransit -- Mediator
        //My intension is to create an API controller that can actually call that 'SubmitOrderConsumer'
        //Go back to the startup and add the request client for the SubmitOrder 'cfg.AddRequestClient<SubmitOrder>();'

        //In this project, we create a consumer inside a web project, so we can decouple the controller from our backend
        //processing logic and the nice thing that this does is it gives a kind of a gateway to
        //to moving towards a fully distributed system. Next step is RabbitMQ and actually break this out of process.

        //It uses a request client to send a SubmitOrder message to a consumer
        //that we created and then based on whether or not the order is accepted or rejected
        //returns the appropiate response code to the http caller

        //Explanation of the logs in the console:
        //1) First we have our actual log message where we say the 'SubmitOrderConsumer' is loggin out.
        //2) We see an additional message, massTransit ReceiveTransport[0], is actually showing that it received
        //   a message of type 'OrderSubmissionAccepted' because we use the request client back on the other side
        //3) And we have the mediator endpoint that received the 'SubmitOrder' message.
        //So this debug messages are all logged out and they are all standard logger factory messages that they
        //can be filtered like anything else.
        //The interest thing to see is that the received message is actually logged after the consumer has completed
        //The response was actually received in processed before it actually finished processing the actual command so
        //a 'SubmitOrder' finished processing after the response was received and returned by the API Controller.
        //So again, the TPL, its interesting to see how the threads are dispatched and how stuff is moved around
        
        
        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            //I'm going to call submitorder and in this case I want a response
            //I'm going to submit the order (send the message) and I'm going to expect some kind of a 
            //handler to get the response back to me. Go to the SubmitOrderConsumer and create an 'OrderSubmissionAccepted' message.
            //In 'Sample.Contracts' project will create 'OrderSubmissionAccepted' interface.
            //Now go to the consumer and add this 'context.RespondAsync<OrderSubmissionAccepted>' in the Consume method to take care of the response message.
            //Some of complains is to know the propeties of this messages, MassTransit has a nuget package
            //MassTransit.Analizer that will do it for you.
            //At this point I creatd an initializer for the message, its going to create a proxy
            //in the background for that message, that SubmitOrderMessage and I'm going to get
            //te response of OrderSubmittionAccepted.
            //Is a proxy because the 'Submitorder' is an interface and MassTransit will dinamically
            //creates a fake class in the background so it can set the properties on it.

            _logger.Log(LogLevel.Debug, "Just before SubmitOrderRequestClient");

            //The way we are configuring the RequestClient is actually publishing the message
            //for SubmitOrder and then getting the response back.

            //With tuples now,  can expect 2 different response types.
            var (accepted,rejected) = await _submitOrderRequestClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new
            {
                OrderId = id,
                InVar.Timestamp,//give the same value if going to use it in different places in this initializer.
                CustomerNumber = customerNumber
            });
            //.Net support tuples now, becuase I have two different response types, I will
            //support different types
            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Accepted(response);
            }
            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }
            //return Ok(response.Message);
        }

        //MediatR
        [HttpGet]
            public async Task<IActionResult> GetAllOrders()
            {
                //MediatR
                //Ideally, every method in the controller should only have this 3 lines
                //the query or command
                var query = new GetAllOrdersQuery();
                //the send method
                var result = await _mediator.Send(query);
                //the result
                return Ok(result);

                //var orders:List<OrderDto> = await _ordersRepository.GetOrdersAsync();
                //var orderResponse:List < OrderResponse > = _mapper.MapOrderDtosToOrderResponse(orders);
                //return Ok(orderResponse);
            }

        //MediatR
        [HttpGet(template:"{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var query = new GetOrderByIdQuery(orderId);
            var result = await _mediator.Send(query);
            return result != null ? (IActionResult) Ok(result) : NotFound();


            //var order = await _orderRepository.GetOrderAsync(orderId);
            //if(order == null)
            //{
            //    return NotFound();
            //}
            //.
            //.
            //.
        }

        ////MediatR
        //[HttpPost]
        //public async Task<IActionResult> CreateOrder([FromBody] CreateCustomerOrderCommand command)
        //{
        //    var result = await _mediator.Send(command);
        //    return CreatedAtAction("GetOrder", new { orderId = result.OrderId }, value: result);
        //}

    }
}

