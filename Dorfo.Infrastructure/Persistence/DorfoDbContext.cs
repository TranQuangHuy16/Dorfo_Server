
using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Persistence
{
    public class DorfoDbContext : DbContext
    {
        public DorfoDbContext()
        {
        }

        public DorfoDbContext(DbContextOptions<DorfoDbContext> options) : base(options) { }

        // DbSets
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Merchant> Merchants => Set<Merchant>();
        public DbSet<MerchantAddress> MerchantAddresses => Set<MerchantAddress>();
        public DbSet<MerchantSetting> MerchantSettings => Set<MerchantSetting>();
        public DbSet<MerchantOpeningDay> MerchantOpeningDays => Set<MerchantOpeningDay>();
        public DbSet<MenuCategory> MenuCategories => Set<MenuCategory>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<MenuItemOption> MenuItemOptions => Set<MenuItemOption>();
        public DbSet<MenuItemOptionValue> MenuItemOptionValues => Set<MenuItemOptionValue>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<OrderItemOption> OrderItemOptions => Set<OrderItemOption>();
        public DbSet<OrderItemOptionValue> OrderItemOptionValues => Set<OrderItemOptionValue>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<LoyaltyAccount> LoyaltyAccounts => Set<LoyaltyAccount>();
        public DbSet<Voucher> Vouchers => Set<Voucher>();
        public DbSet<ChatConversation> ChatConversations => Set<ChatConversation>();
        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

        public DbSet<Shipper> Shippers => Set<Shipper>();
        public DbSet<MerchantCategory> MerchantCategories => Set<MerchantCategory>();

        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ReviewImage> ReviewImages => Set<ReviewImage>();
        public DbSet<ShopReply> ShopReplies => Set<ShopReply>();
        public DbSet<FavoriteShop> FavoriteShops => Set<FavoriteShop>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // PaymentMethods
            modelBuilder.Entity<PaymentMethod>(b =>
            {
                b.HasKey(x => x.PaymentMethodId);
                b.Property(x => x.Code).IsRequired().HasMaxLength(50);
                b.Property(x => x.DisplayName).HasMaxLength(100);
                b.HasIndex(x => x.Code).IsUnique();
            });

            // Users
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.UserId);
                b.Property(x => x.Phone).HasMaxLength(20);
                b.Property(x => x.Email).HasMaxLength(200);
                b.Property(x => x.DisplayName).HasMaxLength(200);
                b.Property(x => x.BirthDate).HasColumnType("date");
                b.Property(x => x.Gender).HasConversion<int?>();
                b.Property(x => x.AvatarUrl).HasMaxLength(1000);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.Username).HasMaxLength(100);
                b.Property(x => x.Password).HasMaxLength(200);

                // unique index on Phone when not null
                b.HasIndex(x => x.Phone).IsUnique().HasFilter("[Phone] IS NOT NULL").HasDatabaseName("UX_Users_Phone");
            });



            // Addresses
            modelBuilder.Entity<Address>(b =>
            {
                b.HasKey(x => x.AddressId);

                b.Property(x => x.AddressLabel).HasMaxLength(200);
                b.Property(x => x.Street).HasMaxLength(300).IsRequired();
                b.Property(x => x.Ward).HasMaxLength(100);
                b.Property(x => x.District).HasMaxLength(100);
                b.Property(x => x.City).HasMaxLength(100);
                b.Property(x => x.Country).HasMaxLength(100).HasDefaultValue("Vietnam");

                b.Property(x => x.IsDefault).HasDefaultValue(false);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasOne(x => x.User)
                    .WithMany(u => u.Addresses)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // Merchants
            modelBuilder.Entity<Merchant>(b =>
            {
                b.HasKey(x => x.MerchantId);
                b.Property(x => x.Name).HasMaxLength(300).IsRequired();
                b.Property(x => x.Description).HasMaxLength(1000);
                b.Property(x => x.Phone).HasMaxLength(50);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.IsOpen).HasDefaultValue(true);
                b.Property(x => x.CommissionRate).HasColumnType("decimal(5,2)").HasDefaultValue(0m);
                b.Property(x => x.ImgUrl).HasMaxLength(1000);


                b.HasOne(x => x.OwnerUser)
                    .WithMany()
                    .HasForeignKey(x => x.OwnerUserId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasMany(x => x.OpeningDays)
                    .WithOne(d => d.Merchant)
                    .HasForeignKey(d => d.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MerchantAddresses
            modelBuilder.Entity<MerchantAddress>(b =>
            {
                b.HasKey(x => x.MerchantAddressId);
                b.Property(x => x.StreetNumber).HasMaxLength(50);
                b.Property(x => x.StreetName).HasMaxLength(200);
                b.Property(x => x.Ward).HasMaxLength(100);
                b.Property(x => x.District).HasMaxLength(100);
                b.Property(x => x.City).HasMaxLength(100);

                b.HasOne(x => x.Merchant)
                 .WithOne(m => m.MerchantAddress)
                 .HasForeignKey<MerchantAddress>(x => x.MerchantId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // MerchantSettings
            modelBuilder.Entity<MerchantSetting>(b =>
            {
                b.HasKey(x => x.MerchantSettingId);
                b.Property(x => x.SupportsScheduling).HasDefaultValue(false);
                b.Property(x => x.DeliveryRadiusMeters).HasDefaultValue(3000);

                b.HasIndex(x => x.MerchantId).IsUnique();

                b.HasOne(x => x.Merchant)
                    .WithOne(m => m.MerchantSetting)
                    .HasForeignKey<MerchantSetting>(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MerchantOpeningDays
            modelBuilder.Entity<MerchantOpeningDay>(b =>
            {
                b.HasKey(x => x.MerchantOpeningDayId);

                b.Property(x => x.DayOfWeek)
                    .HasConversion<int>();

                b.Property(x => x.OpenTime).IsRequired();
                b.Property(x => x.CloseTime).IsRequired();
            });


            // MenuCategory
            modelBuilder.Entity<MenuCategory>(b =>
            {
                b.HasKey(x => x.MenuCategoryId);
                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Name).HasMaxLength(1000);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.SortOrder).HasDefaultValue(0);
                b.HasOne(x => x.Merchant).WithMany(m => m.MenuCategories).HasForeignKey(x => x.MerchantId).OnDelete(DeleteBehavior.Cascade);
            });

            //// MenuItem
            //modelBuilder.Entity<MenuItem>(b =>
            //{
            //    b.HasKey(x => x.MenuItemId);
            //    b.Property(x => x.Name).HasMaxLength(300).IsRequired();
            //    b.Property(x => x.Description).HasMaxLength(2000);
            //    b.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
            //    b.Property(x => x.PrepTimeMinutes);
            //    b.Property(x => x.SupportsScheduling).HasDefaultValue(false);
            //    b.Property(x => x.IsAvailable).HasDefaultValue(true);
            //    b.Property(x => x.IsSpecial).HasDefaultValue(false);
            //    b.Property(x => x.MinQty).HasDefaultValue(1);
            //    b.Property(x => x.Tags).HasMaxLength(500);
            //    b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            //    b.HasOne(x => x.Merchant).WithMany(m => m.MenuItems).HasForeignKey(x => x.MerchantId).OnDelete(DeleteBehavior.Cascade);
            //    b.HasOne(x => x.Category).WithMany(c => c.MenuItems).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.SetNull);
            //    b.HasIndex(x => new { x.MerchantId, x.IsAvailable, x.Price }).HasDatabaseName("IX_MenuItems_Merchant_Available");
            //});

            modelBuilder.Entity<MenuItem>(b =>
            {
                b.HasKey(x => x.MenuItemId);
                b.Property(x => x.Name).HasMaxLength(300).IsRequired();
                b.Property(x => x.Description).HasMaxLength(2000);
                b.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(x => x.ImgUrl).HasMaxLength(1000);
                b.Property(x => x.PrepTimeMinutes);
                b.Property(x => x.SupportsScheduling).HasDefaultValue(false);
                b.Property(x => x.IsAvailable).HasDefaultValue(true);
                b.Property(x => x.IsSpecial).HasDefaultValue(false);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                // Direct link to Merchant: NO cascade
                b.HasOne(x => x.Merchant)
                    .WithMany(m => m.MenuItems)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Restrict); // <-- change from Cascade

                // Link to Category: keep SetNull
                b.HasOne(x => x.Category)
                    .WithMany(c => c.MenuItems)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasIndex(x => new { x.MerchantId, x.IsAvailable, x.Price })
                    .HasDatabaseName("IX_MenuItems_Merchant_Available");
            });


            // MenuItemOption
            modelBuilder.Entity<MenuItemOption>(b =>
            {
                b.HasKey(x => x.OptionId);
                b.Property(x => x.OptionName).HasMaxLength(200).IsRequired();
                b.Property(x => x.IsMultipleChoice).HasDefaultValue(false);
                b.Property(x => x.Required).HasDefaultValue(false);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.HasOne(x => x.MenuItem).WithMany(m => m.Options).HasForeignKey(x => x.MenuItemId).OnDelete(DeleteBehavior.Cascade);
            });

            // MenuItemOptionValue
            modelBuilder.Entity<MenuItemOptionValue>(b =>
            {
                b.HasKey(x => x.OptionValueId);
                b.Property(x => x.ValueName).HasMaxLength(200);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.PriceDelta).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                b.HasOne(x => x.Option).WithMany(o => o.Values).HasForeignKey(x => x.OptionId).OnDelete(DeleteBehavior.Cascade);
            });

            // Carts
            modelBuilder.Entity<Cart>(b =>
            {
                b.HasKey(x => x.CartId);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade); // note: User.Orders used but semantic - you may add Cart nav in User if desired
                b.HasIndex(x => x.UserId);
            });

            // CartItems
            modelBuilder.Entity<CartItem>(b =>
            {
                b.HasKey(x => x.CartItemId);
                b.Property(x => x.PriceAtAdd).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(x => x.OptionsJson).HasColumnType("nvarchar(max)");
                b.HasOne(x => x.Cart).WithMany(c => c.Items).HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.MenuItem).WithMany().HasForeignKey(x => x.MenuItemId);
            });

            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(x => x.OrderId);

                b.Property(x => x.OrderRef).HasMaxLength(100).IsRequired();
                b.HasIndex(x => x.OrderRef).IsUnique();

                b.Property(x => x.SubTotal).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(x => x.DeliveryFee).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                b.Property(x => x.ServiceFee).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                b.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                b.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasOne(x => x.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Restrict);


                b.HasOne(x => x.Merchant)
                    .WithMany(m => m.Orders)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.DeliveryAddress)
                    .WithMany()
                    .HasForeignKey(x => x.DeliveryAddressId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(x => x.PaymentMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(x => x.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasIndex(x => new { x.UserId, x.Status }).HasDatabaseName("IX_Orders_User_Status");
                b.HasIndex(x => new { x.MerchantId, x.Status }).HasDatabaseName("IX_Orders_Merchant_Status");
                b.HasIndex(x => x.ScheduledFor).HasDatabaseName("IX_Orders_ScheduledFor");

                b.Property(x => x.IsScheduled)
    .HasComputedColumnSql("CAST(CASE WHEN [ScheduledFor] IS NULL THEN 0 ELSE 1 END AS BIT)", stored: true);


                // ✅ Enum OrderStatusEnum -> int
                b.Property(x => x.Status)
                    .HasConversion<int>();
            });

            modelBuilder.Entity<OrderItemOption>(entity =>
            {
                entity.ToTable("OrderItemOptions"); // 🔥 map rõ ràng tên bảng
                entity.HasKey(e => e.OrderItemOptionId);

                entity.HasOne(e => e.OrderItem)
                      .WithMany(o => o.OrderItemOptions)
                      .HasForeignKey(e => e.OrderItemId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.MenuItemOption)
                      .WithMany()
                      .HasForeignKey(e => e.MenuItemOptionId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<OrderItemOptionValue>(entity =>
            {
                entity.ToTable("OrderItemOptionValues"); // 🔥 map rõ ràng tên bảng
                entity.HasKey(e => e.OrderItemOptionValueId);

                entity.HasOne(e => e.OrderItemOption)
                      .WithMany(o => o.OrderItemOptionValue)
                      .HasForeignKey(e => e.OrderItemOptionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.MenuItemOptionValue)
                      .WithMany()
                      .HasForeignKey(e => e.MenuItemOptionValueId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.PriceDelta).HasPrecision(18, 2);
            });


            modelBuilder.Entity<Payment>(b =>
            {
                b.HasKey(p => p.PaymentId);

                b.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(p => p.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                // ✅ Enum PaymentStatusEnum -> int
                b.Property(p => p.Status)
                    .HasConversion<int>();
                b.HasOne(p => p.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(p => p.PaymentMethod)
                    .WithMany(m => m.Payments)
                    .HasForeignKey(p => p.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // OrderItems
            modelBuilder.Entity<OrderItem>(b =>
            {
                b.HasKey(x => x.OrderItemId);
                b.Property(x => x.ItemName).HasMaxLength(300).IsRequired();
                b.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                b.Property(x => x.SubTotal).HasColumnType("decimal(18,2)").IsRequired();
                b.HasOne(x => x.Order).WithMany(o => o.Items).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.MenuItem).WithMany().HasForeignKey(x => x.MenuItemId).OnDelete(DeleteBehavior.Restrict);
            });


            // OrderStatusHistory
            modelBuilder.Entity<OrderStatusHistory>(b =>
            {
                b.HasKey(x => x.HistoryId);
                b.Property(x => x.Note).HasMaxLength(1000);
                b.Property(x => x.ChangedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Order).WithMany(o => o.StatusHistory).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.ChangedByUser).WithMany().HasForeignKey(x => x.ChangedByUserId).OnDelete(DeleteBehavior.SetNull);
            });

            // Tickets
            modelBuilder.Entity<Ticket>(b =>
            {
                b.HasKey(x => x.TicketId);
                b.Property(x => x.IssueType).HasMaxLength(200).IsRequired();
                b.Property(x => x.Description).HasMaxLength(2000);
                b.Property(x => x.Status).HasMaxLength(50).HasDefaultValue("NEW");
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            // LoyaltyAccounts
            modelBuilder.Entity<LoyaltyAccount>(b =>
            {
                b.HasKey(x => x.LoyaltyAccountId);
                b.Property(x => x.Points).HasDefaultValue(0);
                b.Property(x => x.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasIndex(x => x.UserId).IsUnique();
                b.HasOne(x => x.User).WithOne(u => u.LoyaltyAccount).HasForeignKey<LoyaltyAccount>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            // Vouchers
            modelBuilder.Entity<Voucher>(b =>
            {
                b.HasKey(x => x.VoucherId);
                b.Property(x => x.Code).HasMaxLength(100).IsRequired();
                b.HasIndex(x => x.Code).IsUnique();
                b.Property(x => x.DiscountAmount).HasColumnType("decimal(18,2)");
                b.Property(x => x.DiscountPercent).HasColumnType("decimal(5,2)");
                b.Property(x => x.MinOrderAmount).HasColumnType("decimal(18,2)");
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Merchant).WithMany(m => m.Vouchers).HasForeignKey(x => x.MerchantId).OnDelete(DeleteBehavior.SetNull);
            });

            // ChatConversation
            modelBuilder.Entity<ChatConversation>(b =>
            {
                b.HasKey(x => x.ConversationId);
                b.Property(x => x.Subject).HasMaxLength(300);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            });

            // ChatMessage
            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasKey(x => x.MessageId);
                b.Property(x => x.MessageText).HasMaxLength(4000);
                b.Property(x => x.Attachments).HasColumnType("nvarchar(max)");
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasOne(x => x.Conversation).WithMany(c => c.Messages).HasForeignKey(x => x.ConversationId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.FromUser).WithMany().HasForeignKey(x => x.FromUserId).OnDelete(DeleteBehavior.SetNull);
            });

            // Shipper
            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.HasKey(s => s.ShipperId);


                entity.HasOne(s => s.Merchant)
                      .WithMany(m => m.Shippers)
                      .HasForeignKey(s => s.MerchantId)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(s => s.User)
                      .WithOne()
                      .HasForeignKey<Shipper>(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(s => s.VehicleType).HasMaxLength(50);
                entity.Property(s => s.LicensePlate).HasMaxLength(20);

                entity.Property(s => s.CccdFrontUrl).HasMaxLength(255);
                entity.Property(s => s.CccdBackUrl).HasMaxLength(255);

                entity.Property(s => s.IsActive).HasDefaultValue(true);
                entity.Property(s => s.IsOnline).HasDefaultValue(false);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            });

            modelBuilder.Entity<MerchantCategory>(entity =>
            {
                entity.HasKey(mc => mc.MerchantCategoryId);

                entity.Property(mc => mc.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(mc => mc.Description)
                      .HasMaxLength(255);

                entity.Property(mc => mc.IsActive)
                  .HasDefaultValue(true);

                entity.Property(mc => mc.ImageUrl)
                    .HasMaxLength(255); // giới hạn độ dài URL, tuỳ bạn

                // Quan hệ 1-n: Category - Merchants
                entity.HasMany(mc => mc.Merchants)
                      .WithOne(m => m.MerchantCategory)
                      .HasForeignKey(m => m.MerchantCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===========================
            // Reviews
            // ===========================
            modelBuilder.Entity<Review>(b =>
            {
                b.HasKey(x => x.ReviewId);
                b.Property(x => x.Comment).HasMaxLength(2000);
                b.Property(x => x.SentAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasOne(x => x.Customer)
                    .WithMany() // nếu bạn có List<Review> trong User, có thể đổi sang .WithMany(u => u.Reviews)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // 🔹 Liên kết với Merchant
                b.HasOne(x => x.Merchant)
                    .WithMany() // hoặc .WithMany(m => m.Reviews) nếu Merchant có List<Review>
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===========================
            // ReviewImages
            // ===========================
            modelBuilder.Entity<ReviewImage>(b =>
            {
                b.HasKey(x => x.ReviewImageId);
                b.Property(x => x.ImgUrl).HasMaxLength(1000).IsRequired();

                b.HasOne(x => x.Review)
                    .WithMany(r => r.Images)
                    .HasForeignKey(x => x.ReviewId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===========================
            // ShopReplies
            // ===========================
            modelBuilder.Entity<ShopReply>(b =>
            {
                b.HasKey(x => x.ShopReplyId);
                b.Property(x => x.Message).HasMaxLength(2000).IsRequired();
                b.Property(x => x.RepliedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasOne(x => x.Review)
                .WithOne(r => r.ShopReply)
                .HasForeignKey<ShopReply>(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Merchant)
                    .WithMany() // nếu Merchant có List<ShopReply> thì đổi thành .WithMany(m => m.Replies)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===========================
            // FavoriteShops
            // ===========================
            modelBuilder.Entity<FavoriteShop>(b =>
            {
                b.HasKey(x => x.FavoriteShopId);
                b.Property(x => x.AddedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasOne(x => x.Customer)
                    .WithMany() // nếu User có List<FavoriteShop> thì đổi thành .WithMany(u => u.FavoriteShops)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Merchant)
                    .WithMany() // hoặc .WithMany(m => m.FavoritedBy)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Mỗi khách hàng chỉ có thể yêu thích 1 shop 1 lần
                b.HasIndex(x => new { x.CustomerId, x.MerchantId }).IsUnique();
            });


            // Enum -> string hoặc int
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>(); // Lưu số int vào DB

            //modelBuilder.Entity<Order>()
            //    .Property(o => o.Status)
            //    .HasConversion<int>(); // Lưu số int vào DB

            //modelBuilder.Entity<Payment>()
            //    .Property(p => p.Status)
            //    .HasConversion<int>();            // enum -> int trong DB


            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { PaymentMethodId = 1, Code = "COD", DisplayName = "Cash On Delivery" },
                new PaymentMethod { PaymentMethodId = 2, Code = "BANK_TRANSFER", DisplayName = "Bank Transfer" }
            );
        }
    }
}
