using MagicLink.Shared.Models;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicLink.Worker;

internal interface IEmailService
{
    Task<Response> SendEmail(Message? message);
}