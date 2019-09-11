using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToDo.Infrastructure;

namespace ToDoApi.Services
{
    public class ShareExpiredService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ShareExpiredService> _logger;

        public ShareExpiredService(IServiceProvider serviceProvider, ILogger<ShareExpiredService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ShareExpiredService started!");
            _timer = new Timer(CheckForExpiredShares, null, TimeSpan.Zero, TimeSpan.FromHours(0.5));

            return Task.CompletedTask;
        }

        private void CheckForExpiredShares(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();
                _context.ToDoListShares.RemoveRange(_context.ToDoListShares.Where(s => DateTime.Compare(s.ExpiresOn, DateTime.Now) <= 0));
                _context.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            _logger.LogInformation("ShareExpiredService ended!");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
