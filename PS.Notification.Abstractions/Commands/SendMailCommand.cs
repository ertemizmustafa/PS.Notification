using System.Collections.Generic;

namespace PS.Notification.Abstractions.Commands
{
    public class SendMailCommand
    {
        public string FromDisplayName { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IEnumerable<string> To { get; set; }
        public IEnumerable<string> Cc { get; set; }
    }
}
