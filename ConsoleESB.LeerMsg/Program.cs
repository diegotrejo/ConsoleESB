using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace ConsoleESB.LeerMsg
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // the client that owns the connection and can be used to create senders and receivers
            ServiceBusClient client;

            // the processor that reads and processes messages from the queue
            ServiceBusProcessor processor;

            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Set the transport type to AmqpWebSockets so that the ServiceBusClient uses port 443. 
            // If you use the default AmqpTcp, make sure that ports 5671 and 5672 are open.

            // TODO: Replace the <NAMESPACE-CONNECTION-STRING> and <QUEUE-NAME> placeholders
            var clientOptions = new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            client = new ServiceBusClient("Endpoint=sb://esbutn001.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SHyDWp51DK1JPKi3Tn5bdIBZ9Tayj8ilO+ASbJlRock=", clientOptions);

            // create a processor that we can use to process the messages
            // TODO: Replace the <QUEUE-NAME> placeholder
            processor = client.CreateProcessor("solicitudes01", new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
            Console.ReadKey();
        }

        // handle received messages
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string json = args.Message.Body.ToString();
            //Console.WriteLine($"Received: {json}");

            /*
             * AQUI DEBE ESCRIBIRSE LAS LINEAS DE CODIGO PARA PROCESAR EL MENSAJE
             */
            var peticion = Newtonsoft.Json.JsonConvert.DeserializeObject<LibreriaESB2.PeticionESB>(json);
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("Peticion: " + args.Message.SequenceNumber);
            Console.WriteLine("Usuario: " + peticion.Usuario);
            Console.WriteLine("IdProdcuto: " + peticion.IdProducto);
            Console.WriteLine("Nombre Prodcuto: " + peticion.NombreProducto);

            // con el objeto peticion deberìa hacer las afectaciones de inventariom crear facturas
            // registros contables, etc etc etc

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }


        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}
