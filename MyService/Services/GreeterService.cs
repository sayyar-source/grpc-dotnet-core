using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace MyService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
           await Task.Delay(3000);
            return new HelloReply
            {
                Message = "Hello from " + request.Name
            };
        }
        public override async Task<HelloReplyList> SayHelloList(HelloRequest request, ServerCallContext context)
        {
            await Task.Delay(1000);
            var list = new HelloReplyList();
            for (int i = 0; i < 10; i++)
            {
                list.List.Add(new HelloReply
                {
                    Message = "Hello Muhammed " + i
                });
            }

            return list;
        }
    }
}
