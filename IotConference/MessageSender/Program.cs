using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace IotConference.MessageSender
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine(
                    "USAGE: msend [Event Hub Name] [Connection String] [Interval (milliseconds)] [Number of messages to send]");
                return;
            }

            var parsed = int.TryParse(args[2], out var interval);

            if (!parsed)
            {
                Console.WriteLine("Interval value must be a number.");
                return;
            }

            int[] intervalRange = {100, 1000};

            if (interval < intervalRange[0] || interval > intervalRange[1])
            {
                Console.WriteLine($"Interval value must be a number between {intervalRange[0]} and {intervalRange[1]}.");
                return;
            }

            parsed = int.TryParse(args[3], out var messageCount);

            if (!parsed)
            {
                Console.WriteLine("Number of messages value must be a number.");
                return;
            }

            int[] messageCountRange = {1, 1000};

            if (messageCount < messageCountRange[0] || messageCount > messageCountRange[1])
            {
                Console.WriteLine($"Number of messages value must be a number between {messageCountRange[0]} and {messageCountRange[1]}.");
                return;
            }


            var eventHubName = args[0];
            var connectionString = args[1];


            Console.WriteLine($"Event hub name: {eventHubName}");
            Console.WriteLine($"Connection string: {connectionString}");
            Console.WriteLine($"Interval: {interval}");
            Console.WriteLine($"Number of messages: {messageCount}");


            MainAsync(eventHubName, connectionString, TimeSpan.FromMilliseconds(interval), messageCount).GetAwaiter()
                .GetResult();
        }

        private static async Task MainAsync(string eventHubName, string connectionString, TimeSpan interval,
            int messageCount)
        {
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
            {
                EntityPath = eventHubName
            };

            var client = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            var messageService = new MessageService();

            try
            {
                for (var i = 0; i < messageCount; i++)
                {
                    var message = messageService.CreateMessage();
                    await client.SendAsync(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));
                    Console.Write($"\rSent message {i + 1} of {messageCount}");
                    await Task.Delay(interval);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {
                await client.CloseAsync();
            }
        }
    }
}