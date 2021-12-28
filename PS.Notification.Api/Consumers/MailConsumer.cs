using MassTransit;
using Microsoft.Extensions.Logging;
using PS.Notification.Abstractions.Commands;
using PS.Notification.Api.Interfaces;
using System.Threading.Tasks;

namespace PS.Notification.Api.Consumers
{
    public class MailConsumer : IConsumer<SendMailCommand>
    {
        private readonly IEMailService _mailService;
        private readonly ILogger<MailConsumer> _logger;


        public MailConsumer(IEMailService mailService, ILogger<MailConsumer> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendMailCommand> context)
        {

            _logger.LogInformation("Value: {Message}", context.Message);
            await _mailService.SendAsync(context.Message);
        }
    }
}
