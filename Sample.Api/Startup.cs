using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Components.Consumers;
using Sample.Contracts;

namespace Sample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

            //Chris Patterson - Main contributor
            //Mass Transit is a lightweight service bus or kind of a distributed application
            //framework for the .Net runtime (core or framework) is built in c#.

            //Getting massTransit in our project, everything is deploy through Nuggets, we are going to install 
            //MassTransit.AspNetCore, is a project thats made to integrate with Web Api projects.
            //It references everything you will need beside of your 'Transport'. It will have your dependency Injection
            //it will have your logging, etc.
                        
            //use the MassTransit extension methods that are part of the 
            //MassTransit Asp .Net Core project.
            //This will allow me to add consumers, add sagas, any of the message handlers that I want
            //in the project. MassTransit has support for a number of different transport, it has an in memory
            //transport, it support RabbitMQ, ActiveMQ as your service bus and Amazon Sqs.

            services.AddMassTransit(cfg =>
            {
                //1) First need to create a message contract (interface) (Sample.Contracts project and call it 'SubmitOrder')
                //2) Create a consumer for that 'SubmitOrder' message. (Sample.Components project and call it 'SubmitOrderConsumer')

                //We put a consumer in here for our SubmitOrderConsumer
                //Comment when using RabbitMQ (taking the message consumer out of process and put it in a separate service
                //cfg.AddConsumer<SubmitOrderConsumer>();

                //We added Mediator, is entirelly in memory version of MassTransit runtime
                //let you register consumers,everything you want inside the service
                //Is a memory way of dispatching without a transport at all. MassTransit has an in memory
                //Transport, but mediator doesnt even use a transport, so no serialization, it makes
                //it extremely fast.
                //cfg.AddMediator();

                ////Using RabbitMQ
                cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq());

                //The purpose of the MassTransit service is to be listening to the Queue in RabbitMQ
                //Is where the consumers are.
                
                //We added a request client for the SubmitOrder
                //(register a client that knows how to send a 'Submitorder' request
                //The way we are configuring the RequestClient is actually publishing the message
                //for SubmitOrder and then getting the response back. 
                //Because is no Address specify, is publishing the message.
                //But if we pass the uri of the appropiate service that is going to process that message (doing a 'Send').
                //In this case I'm going to send the mesage to a Queue, so Im going to call it a Queue
                //cfg.AddRequestClient<SubmitOrder>(
                
                //if you go to the console will see that is a 'Send' this time, to 'Submit-Order'.
                //And will see in the console that 'Bind = true' and is because I use 'queue', and using 'queue' means
                //that if the exchange or queue doenst exist, is going to create the exchange and because 'bind = true' is going to 
                //create the 'queue' and bind 'SubmitOrder' to that queue of the same name.
                //new Uri($"queue:{KebabCaseEndpointNameFormatter.Instance.Consumer<SubmitOrderConsumer>()}"
                
                //Changing to 'exchange', is still a 'send' to 'submit-order', but there is no 'bind', which means
                //that is going to send it to the 'exchange'.
                //new Uri($"exchange:{KebabCaseEndpointNameFormatter.Instance.Consumer<SubmitOrderConsumer>()}"
                //);
                cfg.AddRequestClient<SubmitOrder>();
            });

            //RabbitMQ
            //services.AddMassTransitHostedService();

            //CQRS 
            //this will automatically scan for handlers and register them in DI
            services.AddMediatR(typeof(Startup));

            //swagger
            services.AddOpenApiDocument(cfg => cfg.PostProcess = d => d.Info.Title = "Sample API Site");

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseOpenApi();//serve OpenAPI/swagger documents
            app.UseSwaggerUi3(); //serve Swagger UI

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
