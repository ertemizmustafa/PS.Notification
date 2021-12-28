using PS.Notification.Abstractions.Commands;
using PS.Notification.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PS.Notification.Api.Interfaces
{
    public interface IEMailService
    {
        Task SendAsync(SendMailCommand mailModel);
    }
}
