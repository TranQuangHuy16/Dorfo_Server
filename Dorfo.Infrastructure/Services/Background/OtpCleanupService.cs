using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dorfo.Infrastructure.Persistence;
using Dorfo.Domain.Entities;

namespace Dorfo.Infrastructure.Services.Background
{
    public class OtpCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public OtpCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextRun = DateTime.Today.AddDays(1).AddHours(2); // chạy lúc 2:00 sáng
                var delay = nextRun - DateTime.Now;

                if (delay < TimeSpan.Zero)
                    delay = TimeSpan.FromHours(24); // nếu lỡ giờ thì chờ đến hôm sau

                await Task.Delay(delay, stoppingToken);

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<DorfoDbContext>();

                        var expiredOtps = await db.Otps
                            .Where(o => o.ExpiredAt < DateTime.UtcNow)
                            .ToListAsync(stoppingToken);

                        if (expiredOtps.Any())
                        {
                            db.Otps.RemoveRange(expiredOtps);
                            await db.SaveChangesAsync(stoppingToken);
                            Console.WriteLine($"[OtpCleanupService] Removed {expiredOtps.Count} expired OTP codes.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // log lỗi nếu cần
                    Console.WriteLine($"[OtpCleanupService] Error: {ex.Message}");
                }
            }
        }
    }
}
