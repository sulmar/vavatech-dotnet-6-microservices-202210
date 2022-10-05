using Core.Domain;
using System.Security.Principal;

namespace TrackingService.Domain
{
    public class Message : Base
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}