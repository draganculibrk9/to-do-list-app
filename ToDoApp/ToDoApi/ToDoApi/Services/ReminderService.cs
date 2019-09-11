using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDo.Infrastructure;
using ToDoApi.Options;

namespace ToDoApi.Services
{
    public class ReminderService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ReminderServiceOptions _options;
        private SendGridClient _client;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReminderService> _logger;


        public ReminderService(IOptions<ReminderServiceOptions> options, IServiceProvider serviceProvider, ILogger<ReminderService> logger)
        {
            _options = options.Value;
            _client = new SendGridClient(_options.ApiKey);
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ReminderService started!");
            _timer = new Timer(CheckReminders, null, TimeSpan.Zero, TimeSpan.FromSeconds(_options.ReminderInterval));

            return Task.CompletedTask;
        }

        private void CheckReminders(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
                List<ToDoList> expiredReminders = _context.ToDoLists.Where(l => !l.Reminded && DateTime.Compare(DateTime.Now, l.ReminderDate) > 0).ToList();

                _logger.LogInformation($"ReminderService found {expiredReminders.Count} ToDoLists");

                foreach (ToDoList list in expiredReminders)
                {
                    list.Reminded = true;
                    SendEmail(list.Id, list.Owner);
                }
                _context.SaveChanges();
            }
        }

        private void SendEmail(Guid listId, string owner)
        {
            SendGridMessage msg = MailHelper.CreateSingleEmail(
                new EmailAddress(_options.Email, "To Do Reminder Service"),
                new EmailAddress(owner, "User"),
                _options.Subject, "",
                string.Format(_options.Content, listId.ToString()));

            _client.SendEmailAsync(msg);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            _logger.LogInformation("ReminderService ended!");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
