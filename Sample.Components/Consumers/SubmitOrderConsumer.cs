using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Components.Consumers
{
    //It will implemet the IConsumer(from massTransit -- in this project will need to install 
    //from nugget just the MassTransit (not the Masstransit.AspNetCore package) , i dont want any transport stuff or 
    //anything tied to asp.net.core, I just want stuff related to masstransit )
    //and consume the SubmitOrder message.
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;

        //lets add this logger to visualize how 
        //Consumers run, they run in their own thread, they are running in
        //response to a message, they are invoked by a masstransit, so its kind 
        //of the Hollywood principle, don't call us, we call you. Just like an 
        //API controller or anything on ASP.Net, must people are kind of used to this model now
        //where you create a class, you put some methods on it and then the runtime is responsible for invoking
        //your methods passing you the data.
        //That's exactly whats happening to the consumer. It's actually going out
        //to the DI container, it's creating a new instance of the consumer, passing any dependencies that it has
        //and then calling the consume method on that consumer.
        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            _logger.Log(LogLevel.Debug, "SubmitOrderConsumer: {CustomerNumber}", context.Message.CustomerNumber);
            
            if(context.Message.CustomerNumber.Contains("TEST"))
            {   
                //MassTransit has the ability to intialize this message using an 
                //anonimous initializer.
                //Some of complains is to know the propeties of this messages, MassTransit has a nuget package
                //MassTransit.Analizer that will do it for you.
                await context.RespondAsync<OrderSubmissionRejected>(new
                {
                    InVar.Timestamp, //give the same value if going to use it in different places in this initializer.
                    context.Message.OrderId,
                    context.Message.CustomerNumber,
                    Reason = $"Test Customer cannot submit the order: {context.Message.CustomerNumber}"
                });
                return;
            }
            await context.RespondAsync<OrderSubmissionAccepted>(new 
            { InVar.Timestamp, 
              context.Message.OrderId,
              context.Message.CustomerNumber
            });
        }
    }
}
