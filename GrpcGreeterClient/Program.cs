using Grpc.Core;
using Grpc.Net.Client;
using MyService;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // The port number(5001) must match the port of the gRPC server.
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new Greeter.GreeterClient(channel);
                var header = new Grpc.Core.Metadata();
                header.Add("agant", "user1");
                var option = new Grpc.Core.CallOptions(header, DateTime.UtcNow.AddMilliseconds(3000));
                var source = new CancellationTokenSource();
                var token = source.Token;
                source.CancelAfter(TimeSpan.FromSeconds(1));
             
                var reply = await client.SayHelloAsync(
                                   // new HelloRequest { Name = "GreeterClient" });
                                   // new HelloRequest { Name = "GreeterClient" }, option);
                                   new HelloRequest { Name = "GreeterClient" }, header,DateTime.UtcNow.AddSeconds(3),token);
                Console.WriteLine("Greeting: " + reply.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (RpcException ex)
            {

                Console.WriteLine(ex.StatusCode);
            }
           
        }
    }
}
