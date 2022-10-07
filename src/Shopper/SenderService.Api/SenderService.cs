using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace SenderService.Api
{
    // dotnet add package Topshelf
    public class SenderService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            Send("Started!");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Send("Stopped.");

            return true;
        }

        private void Send(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} {message}");
        }
    }
}
