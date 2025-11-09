using Dorfo.Application.Interfaces.Services;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _config;

        public NotificationService(IConfiguration config)
        {
            _config = config;

            // Khởi tạo FirebaseApp chỉ 1 lần
            if (FirebaseApp.DefaultInstance == null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Configs", "firebase-service-account.json");
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(path)
                });
            }
        }

        /// <summary>
        /// Gửi notification đến device (fcmToken)
        /// </summary>
        public async Task<bool> SendNotificationAsync(string fcmToken, string title, string body)
        {
            if (string.IsNullOrEmpty(fcmToken))
                return false;

            var message = new Message
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"✅ Notification sent successfully: {response}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send notification: {ex.Message}");
                return false;
            }
        }
    }
}
