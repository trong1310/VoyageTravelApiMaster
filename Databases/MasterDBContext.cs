using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TravelMasterApi.Databases;

public partial class MasterDBContext : DbContext
{
    public MasterDBContext(DbContextOptions<MasterDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookingDetail> BookingDetail { get; set; }

    public virtual DbSet<Bookings> Bookings { get; set; }

    public virtual DbSet<CarRoute> CarRoute { get; set; }

    public virtual DbSet<Cars> Cars { get; set; }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Hotels> Hotels { get; set; }

    public virtual DbSet<Images> Images { get; set; }

    public virtual DbSet<Locations> Locations { get; set; }

    public virtual DbSet<ManagerAccounts> ManagerAccounts { get; set; }

    public virtual DbSet<TourDestination> TourDestination { get; set; }

    public virtual DbSet<Tours> Tours { get; set; }

    public virtual DbSet<UserAuthentications> UserAuthentications { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_uca1400_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("booking_detail")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.BookingUuid, "booking_uuid");

            entity.HasIndex(e => e.CreatedAt, "created_at");

            entity.HasIndex(e => e.IsEnable, "is_enable");

            entity.HasIndex(e => e.PhoneNumber, "phone_number");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.BookingUuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("booking_uuid");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.EndTime)
                .HasColumnType("timestamp")
                .HasColumnName("end_time");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.SpecialRequirements)
                .HasColumnType("text")
                .HasColumnName("special_requirements");
            entity.Property(e => e.StartTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("start_time");
            entity.Property(e => e.TotalCustomer)
                .HasColumnType("int(11)")
                .HasColumnName("total_customer");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");

            entity.HasOne(d => d.BookingUu).WithMany(p => p.BookingDetail)
                .HasPrincipalKey(p => p.Uuid)
                .HasForeignKey(d => d.BookingUuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_detail_ibfk_4");
        });

        modelBuilder.Entity<Bookings>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("bookings")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.CategoryId, "category_id");

            entity.HasIndex(e => e.CreatedAt, "created_at");

            entity.HasIndex(e => e.IsEnable, "is_enable");

            entity.HasIndex(e => e.State, "state");

            entity.HasIndex(e => e.Type, "type");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("bigint(20)")
                .HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.SlugOwner)
                .HasMaxLength(255)
                .HasComment("tour  theo yêu cầu thì không có slug")
                .HasColumnName("slug_owner");
            entity.Property(e => e.State)
                .HasComment("PendingProcessing = 1,\r\nProcessed = 2,\r\nCancelled = 3,")
                .HasColumnType("tinyint(4)")
                .HasColumnName("state");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("double(20,0)")
                .HasColumnName("total_price");
            entity.Property(e => e.Type)
                .HasComment("1: Fixed Tours , 2: Custom Tours, != tour = 0")
                .HasColumnType("tinyint(4)")
                .HasColumnName("type");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");

            entity.HasOne(d => d.Category).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("bookings_ibfk_1");
        });

        modelBuilder.Entity<CarRoute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("car_route")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.SlugCar, "idx_slug_car");

            entity.HasIndex(e => e.SlugLocations, "idx_slug_locations");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.DisplayOrder)
                .HasColumnType("int(11)")
                .HasColumnName("display_order");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.SlugCar).HasColumnName("slug_car");
            entity.Property(e => e.SlugLocations).HasColumnName("slug_locations");

            entity.HasOne(d => d.SlugCarNavigation).WithMany(p => p.CarRoute)
                .HasPrincipalKey(p => p.Slug)
                .HasForeignKey(d => d.SlugCar)
                .HasConstraintName("fk_car_route_car");

            entity.HasOne(d => d.SlugLocationsNavigation).WithMany(p => p.CarRoute)
                .HasPrincipalKey(p => p.Slug)
                .HasForeignKey(d => d.SlugLocations)
                .HasConstraintName("fk_car_route_location");
        });

        modelBuilder.Entity<Cars>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("cars")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.LicensePlate, "license_plate").IsUnique();

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("id")
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .HasComment("hang xe")
                .HasColumnName("brand");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasComment("mau sac")
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasComment("ngay tao")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasComment("mo ta")
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasComment("trang thai")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.IsHot)
                .HasColumnType("bit(1)")
                .HasColumnName("is_hot");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(50)
                .HasComment("bien so")
                .HasColumnName("license_plate");
            entity.Property(e => e.ManufactureYear)
                .HasComment("nam san xuat")
                .HasColumnType("int(11)")
                .HasColumnName("manufacture_year");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ten xe")
                .HasColumnName("name");
            entity.Property(e => e.SeatCount)
                .HasDefaultValueSql("'4'")
                .HasComment("so ghe")
                .HasColumnType("int(11)")
                .HasColumnName("seat_count");
            entity.Property(e => e.Slug).HasColumnName("slug");
            entity.Property(e => e.ThumbNail)
                .HasMaxLength(255)
                .HasComment("anh xe")
                .HasColumnName("thumb_nail");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasComment("ngay cap nhat")
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("categories")
                .UseCollation("utf8mb4_unicode_ci");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Banner)
                .HasMaxLength(255)
                .HasColumnName("banner");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Hotels>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("hotels")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.CreatedAt, "created_at");

            entity.HasIndex(e => e.IsEnable, "is_enable");

            entity.HasIndex(e => e.Ranking, "rank");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.HasIndex(e => e.SlugLocations, "slug_locations");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Introduce).HasColumnName("introduce");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.IsHot)
                .HasColumnType("bit(1)")
                .HasColumnName("is_hot");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Ranking)
                .HasDefaultValueSql("'5'")
                .HasColumnName("ranking");
            entity.Property(e => e.Regulations).HasColumnName("regulations");
            entity.Property(e => e.RelativePrice)
                .HasColumnType("double(20,0)")
                .HasColumnName("relative_price");
            entity.Property(e => e.SalePrice)
                .HasComment("lưu tạm khi có giảm giá để query không cần tính , xóa khi voucher hết hạnx")
                .HasColumnType("double(20,0)")
                .HasColumnName("sale_price");
            entity.Property(e => e.Slug)
                .HasDefaultValueSql("''")
                .HasColumnName("slug");
            entity.Property(e => e.SlugLocations).HasColumnName("slug_locations");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .HasColumnName("thumbnail");
            entity.Property(e => e.Type)
                .HasColumnType("tinyint(4)")
                .HasColumnName("type");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");

            entity.HasOne(d => d.SlugLocationsNavigation).WithMany(p => p.Hotels)
                .HasPrincipalKey(p => p.Slug)
                .HasForeignKey(d => d.SlugLocations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("hotels_ibfk_1");
        });

        modelBuilder.Entity<Images>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("images")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.IsEnable, "is_enable");

            entity.HasIndex(e => new { e.OwnerUuid, e.IsEnable }, "owner_uuid");

            entity.HasIndex(e => e.Path, "path").IsUnique();

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.OwnerUuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("owner_uuid");
            entity.Property(e => e.Path).HasColumnName("path");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<Locations>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("locations")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Name, "name");

            entity.HasIndex(e => new { e.Name, e.Slug }, "name_2");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Images)
                .HasMaxLength(255)
                .HasColumnName("images");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Slug)
                .HasDefaultValueSql("''")
                .HasColumnName("slug");
        });

        modelBuilder.Entity<ManagerAccounts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("manager_accounts")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => new { e.IsEnable, e.State }, "is_enable");

            entity.HasIndex(e => e.UserName, "username").IsUnique();

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.State)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("state");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .HasColumnName("user_name");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<TourDestination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tour_destination")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.LocationSlug, "location_slug");

            entity.HasIndex(e => e.TourSlug, "tour_slug");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.DisplayOrder)
                .HasColumnType("int(5)")
                .HasColumnName("display_order");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.LocationSlug).HasColumnName("location_slug");
            entity.Property(e => e.TourSlug).HasColumnName("tour_slug");

            entity.HasOne(d => d.LocationSlugNavigation).WithMany(p => p.TourDestination)
                .HasPrincipalKey(p => p.Slug)
                .HasForeignKey(d => d.LocationSlug)
                .HasConstraintName("tour_destination_ibfk_2");

            entity.HasOne(d => d.TourSlugNavigation).WithMany(p => p.TourDestination)
                .HasPrincipalKey(p => p.Slug)
                .HasForeignKey(d => d.TourSlug)
                .HasConstraintName("tour_destination_ibfk_1");
        });

        modelBuilder.Entity<Tours>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tours")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.CategoryId, "category_id_2");

            entity.HasIndex(e => e.Slug, "slug").IsUnique();

            entity.HasIndex(e => e.DepartureSlug, "tour_ibfk_3");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("bigint(20)")
                .HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("created_by");
            entity.Property(e => e.DepartureSlug)
                .HasComment("điểm khởi hành")
                .HasColumnName("departure_slug");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DurationDays)
                .HasColumnType("int(11)")
                .HasColumnName("duration_days");
            entity.Property(e => e.DurationNights)
                .HasColumnType("int(11)")
                .HasColumnName("duration_nights");
            entity.Property(e => e.Highlight)
                .HasColumnType("text")
                .HasColumnName("highlight");
            entity.Property(e => e.Introduce)
                .HasColumnType("text")
                .HasColumnName("introduce");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.IsHot)
                .HasColumnType("bit(1)")
                .HasColumnName("is_hot");
            entity.Property(e => e.OriginalPrices)
                .HasColumnType("double(20,0)")
                .HasColumnName("original_prices");
            entity.Property(e => e.Ranking)
                .HasDefaultValueSql("'5'")
                .HasColumnType("int(11)")
                .HasColumnName("ranking");
            entity.Property(e => e.SalePrices)
                .HasColumnType("double(20,0)")
                .HasColumnName("sale_prices");
            entity.Property(e => e.Slug).HasColumnName("slug");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .HasColumnName("thumbnail");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");

            entity.HasOne(d => d.Category).WithMany(p => p.Tours)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tours_ibfk_1");

            entity.HasOne(d => d.DepartureSlugNavigation).WithMany(p => p.Tours)
                .HasPrincipalKey(p => p.Slug)
                .HasForeignKey(d => d.DepartureSlug)
                .HasConstraintName("tours_ibfk_3");
        });

        modelBuilder.Entity<UserAuthentications>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_authentications");

            entity.HasIndex(e => e.UserUuid, "user_uuid");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.PassWord)
                .HasMaxLength(255)
                .HasColumnName("pass_word");
            entity.Property(e => e.Provider)
                .HasComment("1: local,2: google,3: facebook")
                .HasColumnType("tinyint(4)")
                .HasColumnName("provider");
            entity.Property(e => e.ProviderId)
                .HasMaxLength(255)
                .HasColumnName("provider_id");
            entity.Property(e => e.UserUuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("user_uuid");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");

            entity.HasOne(d => d.UserUu).WithMany(p => p.UserAuthentications)
                .HasPrincipalKey(p => p.Uuid)
                .HasForeignKey(d => d.UserUuid)
                .HasConstraintName("user_authentications_ibfk_1");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.CreatedAt, "created_at");

            entity.HasIndex(e => e.State, "state");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.IsEnable)
                .HasDefaultValueSql("b'1'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_enable");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
            entity.Property(e => e.State)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(2)")
                .HasColumnName("state");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .IsFixedLength()
                .HasColumnName("uuid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
