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
                var reply = await client.SayHelloListAsync(
                                   new HelloRequest { Name = "GreeterClient" });                                  
             
                foreach (var item in reply.List)
                {
                    Console.WriteLine(item.Message);
                }
            }
            catch (RpcException ex)
            {

                Console.WriteLine(ex.StatusCode);
            }
            Console.ReadKey();
           
        }
    }
}
