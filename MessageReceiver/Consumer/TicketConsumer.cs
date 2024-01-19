using MassTransit;
using MessageReceiver.Model;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace MessageReceiver.Consumer
{
    public class TicketConsumer : IConsumer<Tokken>
    {
        public async Task Consume(ConsumeContext<Tokken> context)
        {
            var data = context.Message;
        }
    }
}

