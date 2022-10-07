
// dotnet add package Topshelf
using Topshelf;

HostFactory.Run(h =>
{
    h.Service<SenderService.Api.SenderService>();
    h.SetServiceName("SenderService");
    h.StartAutomatically();
});
