using NetSdrClient;
using NetSdrClient.Consumers;
using NetSdrConsole;

var logger = Extensions.SetupConsoleLogger();


var config = ConnectionConfig.Default();

var tcpMessageConsumer = new TcpMessageConsumer(logger);
using var udpMessageConsumed = new DataMessageConsumer("data.dat", logger);

var netSDRClient = new NetSDRClient(config, logger, tcpMessageConsumer, udpMessageConsumed);

try
{
    await netSDRClient.ConnectAsync();

    await netSDRClient.RequestNameAsync();

    await Task.Delay(1000);
    await netSDRClient.SetFrequencyAsync(20000000);
    await Task.Delay(1000);

    Console.WriteLine("Press Any key to start receiving...");
    Console.ReadLine();
    await netSDRClient.StartReceivingAsync();
    

    Console.WriteLine("Press Any key to stop receiving...");
    Console.ReadLine();

    await netSDRClient.StopReceivingAsync();


    Console.WriteLine("Press Any key to exit...");
    Console.ReadLine();
}
catch
{
    Console.WriteLine("Oh... Something went wrong(");
}
finally
{
    netSDRClient.Disconnect();
}