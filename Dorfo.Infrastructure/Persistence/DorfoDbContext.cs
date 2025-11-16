using Dorfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dorfo.Infrastructure.Persistence
{
    public class DorfoDbContext : DbContext
    {
        public DorfoDbContext() { }

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

            // ================= PaymentMethod =================
            modelBuilder.Entity<PaymentMethod>(b =>
            {
                b.HasKey(x => x.PaymentMethodId);
                b.Property(x => x.Code).IsRequired().HasMaxLength(50);
                b.Property(x => x.DisplayName).HasMaxLength(100);
                b.HasIndex(x => x.Code).IsUnique();
            });

            // ================= User =================
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.UserId);
                b.Property(x => x.UserId).HasColumnType("uuid");

                b.Property(x => x.Phone).HasMaxLength(20);
                b.Property(x => x.Email).HasMaxLength(200);
                b.Property(x => x.DisplayName).HasMaxLength(200);
                b.Property(x => x.BirthDate).HasColumnType("date");
                b.Property(x => x.Gender).HasConversion<int?>();
                b.Property(x => x.AvatarUrl).HasMaxLength(1000);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.Username).HasMaxLength(100);
                b.Property(x => x.Password).HasMaxLength(200);

                // unique index on Phone when not null
                b.HasIndex(x => x.Phone)
                 .IsUnique()
                 .HasFilter("\"Phone\" IS NOT NULL") // ✅ PostgreSQL filter
                 .HasDatabaseName("UX_Users_Phone");
            });

            // ================= Address =================
            modelBuilder.Entity<Address>(b =>
            {
                b.HasKey(x => x.AddressId);
                b.Property(x => x.AddressId).HasColumnType("uuid");

                b.Property(x => x.AddressLabel).HasMaxLength(200);
                b.Property(x => x.Street).HasMaxLength(300).IsRequired();
                b.Property(x => x.Ward).HasMaxLength(100);
                b.Property(x => x.District).HasMaxLength(100);
                b.Property(x => x.City).HasMaxLength(100);
                b.Property(x => x.Country).HasMaxLength(100).HasDefaultValue("Vietnam");
                b.Property(x => x.IsDefault).HasDefaultValue(false);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            });

            // ================= Merchant =================
            modelBuilder.Entity<Merchant>(b =>
            {
                b.HasKey(x => x.MerchantId);
                b.Property(x => x.MerchantId).HasColumnType("uuid");

                b.Property(x => x.Name).HasMaxLength(300).IsRequired();
                b.Property(x => x.Description).HasMaxLength(1000);
                b.Property(x => x.Phone).HasMaxLength(50);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.IsOpen).HasDefaultValue(true);
                b.Property(x => x.CommissionRate).HasColumnType("numeric(5,2)").HasDefaultValue(0m);

                b.HasOne(m => m.MerchantCategory)
                    .WithMany(mc => mc.Merchants)
                    .HasForeignKey(m => m.MerchantCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(m => m.OpeningDays)
                    .WithOne(od => od.Merchant)
                    .HasForeignKey(od => od.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ================= MerchantOpeningDay =================
            modelBuilder.Entity<MerchantOpeningDay>(b =>
            {
                b.HasKey(x => x.MerchantOpeningDayId);
                b.Property(x => x.MerchantOpeningDayId).HasColumnType("uuid");

                b.Property(x => x.DayOfWeek).HasConversion<int>(); // enum -> int
                b.Property(x => x.OpenTime).HasColumnType("interval").IsRequired();
                b.Property(x => x.CloseTime).HasColumnType("interval").IsRequired();

                // Quan hệ N-1 với Merchant
                b.HasOne(x => x.Merchant)
                    .WithMany(m => m.OpeningDays)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ================= MerchantCategory =================
            modelBuilder.Entity<MerchantCategory>(b =>
            {
                b.HasKey(x => x.MerchantCategoryId);
                b.Property(x => x.MerchantCategoryId)
                    .UseIdentityAlwaysColumn(); // PostgreSQL identity column

                b.Property(x => x.Name).HasMaxLength(200).IsRequired();
                b.Property(x => x.Description).HasMaxLength(1000);
                b.Property(x => x.ImageUrl).HasMaxLength(1000);
                b.Property(x => x.IsActive).HasDefaultValue(true);

                // Quan hệ 1-n với Merchant
                b.HasMany(mc => mc.Merchants)
                    .WithOne(m => m.MerchantCategory)
                    .HasForeignKey(m => m.MerchantCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ================= MenuItem =================
            modelBuilder.Entity<MenuItem>(b =>
            {
                b.HasKey(x => x.MenuItemId);
                b.Property(x => x.MenuItemId).HasColumnType("uuid");

                b.Property(x => x.Name).HasMaxLength(300).IsRequired();
                b.Property(x => x.Description).HasMaxLength(2000);
                b.Property(x => x.Price).HasColumnType("numeric(18,2)").IsRequired();
                b.Property(x => x.PrepTimeMinutes);
                b.Property(x => x.SupportsScheduling).HasDefaultValue(false);
                b.Property(x => x.IsAvailable).HasDefaultValue(true);
                b.Property(x => x.IsSpecial).HasDefaultValue(false);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            });

            // ================= MenuItemOption =================
            modelBuilder.Entity<MenuItemOption>(b =>
            {
                b.HasKey(x => x.OptionId);
                b.Property(x => x.OptionId).HasColumnType("uuid");

                b.Property(x => x.OptionName).HasMaxLength(200).IsRequired();
                b.Property(x => x.IsMultipleChoice).HasDefaultValue(false);
                b.Property(x => x.Required).HasDefaultValue(false);
                b.Property(x => x.IsActive).HasDefaultValue(true);
            });

            // ================= MenuItemOptionValue =================
            modelBuilder.Entity<MenuItemOptionValue>(b =>
            {
                b.HasKey(x => x.OptionValueId);
                b.Property(x => x.OptionValueId).HasColumnType("uuid");

                b.Property(x => x.ValueName).HasMaxLength(200);
                b.Property(x => x.PriceDelta).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
                b.Property(x => x.IsActive).HasDefaultValue(true);
            });

            // ================= Order =================
            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(x => x.OrderId);
                b.Property(x => x.OrderId).HasColumnType("uuid");

                b.Property(x => x.SubTotal).HasColumnType("numeric(18,2)").IsRequired();
                b.Property(x => x.DeliveryFee).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
                b.Property(x => x.ServiceFee).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
                b.Property(x => x.DiscountAmount).HasColumnType("numeric(18,2)").HasDefaultValue(0m);
                b.Property(x => x.TotalAmount).HasColumnType("numeric(18,2)").IsRequired();
                b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");

                // ✅ PostgreSQL style computed column
                b.Property(x => x.IsScheduled)
                  .HasComputedColumnSql("CASE WHEN \"ScheduledFor\" IS NULL THEN FALSE ELSE TRUE END", stored: true);

                b.Property(x => x.Status).HasConversion<int>();
            });

            // ================= OrderStatusHistory =================
            modelBuilder.Entity<OrderStatusHistory>(b =>
            {
                b.HasKey(x => x.HistoryId);
                b.Property(x => x.HistoryId).HasColumnType("uuid");

                b.Property(x => x.FromStatus).HasConversion<int>();
                b.Property(x => x.ToStatus).HasConversion<int>();
                b.Property(x => x.ChangedAt).HasDefaultValueSql("NOW()");
            });

            // ================= Payment =================
            modelBuilder.Entity<Payment>(b =>
            {
                b.HasKey(p => p.PaymentId);
                b.Property(p => p.PaymentId).HasColumnType("uuid");

                b.Property(p => p.Amount).HasColumnType("numeric(18,2)").IsRequired();
                b.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
                b.Property(p => p.Status).HasConversion<int>();
            });

            // ================= ChatConversation =================
            modelBuilder.Entity<ChatConversation>(b =>
            {
                b.HasKey(x => x.ConversationId);
                b.Property(x => x.ConversationId).HasColumnType("uuid");

                b.Property(x => x.Subject).HasMaxLength(300);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            });

            // ================= ChatMessage =================
            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasKey(x => x.MessageId);
                b.Property(x => x.MessageId).HasColumnType("uuid");

                b.Property(x => x.MessageText).HasMaxLength(4000);
                b.Property(x => x.Attachments).HasColumnType("text");
                b.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            });

            // ===========================
            // Reviews
            // ===========================
            modelBuilder.Entity<Review>(b =>
            {
                b.HasKey(x => x.ReviewId);
                b.Property(x => x.ReviewId).HasColumnType("uuid");

                b.Property(x => x.Comment).HasMaxLength(2000);
                b.Property(x => x.RatingProduct).HasColumnType("numeric(3,2)").IsRequired(); // nếu có Rating
                b.Property(x => x.SentAt).HasDefaultValueSql("NOW()");

                // Liên kết với Customer (User)
                b.HasOne(x => x.Customer)
                    .WithMany() // hoặc .WithMany(u => u.Reviews)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Liên kết với Merchant
                b.HasOne(x => x.Merchant)
                    .WithMany() // hoặc .WithMany(m => m.Reviews)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===========================
            // ReviewImages
            // ===========================
            modelBuilder.Entity<ReviewImage>(b =>
            {
                b.HasKey(x => x.ReviewImageId);
                b.Property(x => x.ReviewImageId).HasColumnType("uuid");

                b.Property(x => x.ImgUrl)
                    .HasMaxLength(1000)
                    .IsRequired();

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
                b.Property(x => x.ShopReplyId).HasColumnType("uuid");

                b.Property(x => x.Message).HasMaxLength(2000).IsRequired();
                b.Property(x => x.RepliedAt).HasDefaultValueSql("NOW()");

                // Mỗi Review có tối đa 1 phản hồi từ shop
                b.HasOne(x => x.Review)
                    .WithOne(r => r.ShopReply)
                    .HasForeignKey<ShopReply>(x => x.ReviewId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Merchant)
                    .WithMany() // hoặc .WithMany(m => m.Replies)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // ===========================
            // FavoriteShops
            // ===========================
            modelBuilder.Entity<FavoriteShop>(b =>
            {
                b.HasKey(x => x.FavoriteShopId);
                b.Property(x => x.FavoriteShopId).HasColumnType("uuid");

                b.Property(x => x.AddedAt).HasDefaultValueSql("NOW()");

                b.HasOne(x => x.Customer)
                    .WithMany() // hoặc .WithMany(u => u.FavoriteShops)
                    .HasForeignKey(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Merchant)
                    .WithMany() // hoặc .WithMany(m => m.FavoritedBy)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Mỗi khách hàng chỉ có thể yêu thích 1 shop 1 lần
                b.HasIndex(x => new { x.CustomerId, x.MerchantId })
                    .IsUnique()
                    .HasDatabaseName("UX_FavoriteShop_Customer_Merchant");
            });


            // ================= Seed data =================
            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { PaymentMethodId = 1, Code = "COD", DisplayName = "Cash On Delivery" },
                new PaymentMethod { PaymentMethodId = 2, Code = "BANK_TRANSFER", DisplayName = "Bank Transfer" }
            );
        }
    }
}