using WorkerService;


// dotnet add package Microsoft.Extensions.Hosting.Systemd
IHost host = Host.CreateDefaultBuilder(args)
    // .UseWindowsService()
    // .UseSystemd() // Linux https://blog.maartenballiauw.be/post/2021/05/25/running-a-net-application-as-a-service-on-linux-with-systemd.html
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
