using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using grpc_WebApp.Models;
using Grpc.Net.Client;
using MyService;
using System.Threading;
using Grpc.Core;
using Polly;

namespace grpc_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("api/hi")]
        public async Task<IActionResult> Hello()
        {
            try
            {
                // The port number(5001) must match the port of the gRPC server.
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Greeter.GreeterClient(channel);
                var header = new Grpc.Core.Metadata();
                header.Add("agant", "user1");
             
                var source = new CancellationTokenSource();
                var token = source.Token;
              
                var maxRetryAttempts = 10;
                var pauseBetweenFailures = TimeSpan.FromSeconds(3);
                var retryPolicy = Policy
                .Handle<RpcException>()
                .WaitAndRetryAsync(maxRetryAttempts,
                i => pauseBetweenFailures, (ex, pause) => {
                    _logger.LogError(ex.Message + " => " + pause.TotalSeconds);
                });
                HelloReply reply = new HelloReply();
                await retryPolicy.ExecuteAsync(async () => {
                     reply = await client.SayHelloAsync(
                                      
                                       new HelloRequest { Name = "GreeterClient" }, header, DateTime.UtcNow.AddSeconds(3), token);
                

                });
                return Ok(reply.Message);
            }
            catch (RpcException ex)
            {

               return BadRequest(ex.StatusCode);
            }

        }




    }
}
