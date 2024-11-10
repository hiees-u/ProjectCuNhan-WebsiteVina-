using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

public partial class DbVINA : DbContext
{
    private readonly string _connectionString;
    public DbVINA()
    {
    }

    public DbVINA(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DbVINA(DbContextOptions<DbVINA> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Cell> Cells { get; set; }

    public virtual DbSet<Commune> Communes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerType> CustomerTypes { get; set; }

    public virtual DbSet<DeliveryNote> DeliveryNotes { get; set; }

    public virtual DbSet<DeliveryNoteDetail> DeliveryNoteDetails { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PriceHistory> PriceHistories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shelve> Shelves { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseReceipt> WarehouseReceipts { get; set; }

    public virtual DbSet<WarehouseReceiptDetail> WarehouseReceiptDetails { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID=sa;Password=12345;Encrypt=False;Trust Server Certificate=True");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrEmpty(_connectionString))
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        else
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-L6DVGTI;Initial Catalog=CAFFEE_VINA_DBv1;User ID=sa;Password=12345;Encrypt=False;Trust Server Certificate=True");
            // Nếu không có chuỗi kết nối, bạn có thể đặt một chuỗi kết nối mặc định ở đây
            // optionsBuilder.UseSqlServer("YourDefaultConnectionString");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1BB34ABD51");

            entity.ToTable("Address", tb => tb.HasComment("Địa Chỉ"));

            entity.Property(e => e.HouseNumber).HasComment("Tên đường + số nhà");

            entity.HasOne(d => d.Commune).WithMany(p => p.Addresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Address_CommuneID");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.ProductId }).HasName("pk_Cart");

            entity.ToTable("Cart", tb => tb.HasComment("Giỏ Hàng"));

            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Customer).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Cart_Customer");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Product");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B49A53E1EA");

            entity.ToTable("Category", tb => tb.HasComment("Loại Sản Phẩm"));

            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Cell>(entity =>
        {
            entity.HasKey(e => e.CellId).HasName("PK__Cells__EA424A289FE63E33");

            entity.ToTable(tb =>
                {
                    tb.HasComment("Ô chứa sản phẩm");
                    tb.HasTrigger("updateQuantityProduct");
                });

            entity.Property(e => e.Quantity).HasDefaultValue(0);

            entity.HasOne(d => d.Product).WithMany(p => p.Cells).HasConstraintName("fk_Cells_ProductID");

            entity.HasOne(d => d.Shelves).WithMany(p => p.Cells)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cells_Shelve");
        });

        modelBuilder.Entity<Commune>(entity =>
        {
            entity.HasKey(e => e.CommuneId).HasName("PK__Commune__3E2EBD72DC66DB6E");

            entity.ToTable("Commune", tb => tb.HasComment("Xã/Phường"));

            entity.HasOne(d => d.District).WithMany(p => p.Communes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Commune_DistrictID");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__B611CB7DA15E5D5B");

            entity.ToTable("Customer", tb => tb.HasComment("Khách Hàng"));

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.TypeCustomer).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__type_c__1387E197");
        });

        modelBuilder.Entity<CustomerType>(entity =>
        {
            entity.HasKey(e => e.TypeCustomerId).HasName("PK__Customer__9688D000494C78C0");

            entity.ToTable("CustomerType", tb => tb.HasComment("Loại Khách Hàng"));
        });

        modelBuilder.Entity<DeliveryNote>(entity =>
        {
            entity.HasKey(e => e.DeliveryNoteId).HasName("PK__Delivery__2A1CDD7E4683773D");

            entity.ToTable("DeliveryNote", tb => tb.HasComment("Phiếu Xuất Kho"));

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.DeliveryNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeliveryNote_Employee");

            entity.HasOne(d => d.Order).WithMany(p => p.DeliveryNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DeliveryNote_Order");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.DeliveryNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DeliveryNote_Warehouse");
        });

        modelBuilder.Entity<DeliveryNoteDetail>(entity =>
        {
            entity.HasKey(e => new { e.DeliveryNote, e.CellId }).HasName("pk_DeliveryNoteDetail");

            entity.ToTable("DeliveryNoteDetail", tb =>
                {
                    tb.HasComment("Chi Tiết Phiếu Xuất Kho");
                    tb.HasTrigger("trg_validate_cell_for_warehouse");
                });

            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Cell).WithMany(p => p.DeliveryNoteDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DeliveryNoteDetail_CellID");

            entity.HasOne(d => d.DeliveryNoteNavigation).WithMany(p => p.DeliveryNoteDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DeliveryNoteDetail_DeliveryNote");

            entity.HasOne(d => d.Product).WithMany(p => p.DeliveryNoteDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DeliveryNoteDetail_product");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCDB1CBEB48");

            entity.ToTable("Department", tb => tb.HasComment("Phòng Ban"));
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PK__District__85FDA4A6C41F26C3");

            entity.ToTable("District", tb => tb.HasComment("Quận/Huyện"));

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_District_ProvinceID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF172119C3D");

            entity.ToTable("Employee", tb => tb.HasComment("Nhân Viên"));

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Employee_Department");

            entity.HasOne(d => d.EmployeeType).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Employee_EmployeeType");
        });

        modelBuilder.Entity<EmployeeType>(entity =>
        {
            entity.HasKey(e => e.EmployeeTypeId).HasName("PK__Employee__1F1B6AB4F2989344");

            entity.ToTable("EmployeeType", tb => tb.HasComment("Chức Vụ"));

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__ORDER___F1E4639B3ACA4972");

            entity.ToTable("Order", tb => tb.HasComment("Đơn Đặt Hàng Của Khách"));

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Adress).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Adress_ORDER");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.PriceHistoryId }).HasName("FK_ORDER_DETAIL");

            entity.ToTable("OrderDetail", tb => tb.HasComment("Chi Tiết Đơn Đặt Hàng Của Khách"));

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Order_ORDER_DETAIL");

            entity.HasOne(d => d.PriceHistory).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_PriceHistory");
        });

        modelBuilder.Entity<PriceHistory>(entity =>
        {
            entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__77D1486C998C8C26");

            entity.ToTable("PriceHistory", tb => tb.HasComment("Lịch sử giá"));

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Product).WithMany(p => p.PriceHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PriceHist__produ__5B0E7E4A");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__PRODUCT__47027DF5C0DF35B9");

            entity.ToTable("Product", tb => tb.HasComment("Sản Phẩm"));

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TotalQuantity).HasDefaultValue(0);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Category_PRODUCT");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUCT_SubCategory");

            entity.HasOne(d => d.SupplierNavigation).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Manufacturer_PRODUCT");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__FD0A6FA3067C8CA8");

            entity.ToTable("Province", tb => tb.HasComment("Tỉnh/Thành Phố"));
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId).HasName("PK__Purchase__036BAC44EE2BFA95");

            entity.ToTable("PurchaseOrder", tb => tb.HasComment("Đơn đặt hàng của Công ty"));

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrder_Employee");
        });

        modelBuilder.Entity<PurchaseOrderDetail>(entity =>
        {
            entity.ToTable("PurchaseOrderDetail", tb =>
                {
                    tb.HasComment("Chi tiết đơn đặt hàng của Công Ty");
                    tb.HasTrigger("trg_CheckAddressID");
                });

            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.QuantityDelivered).HasDefaultValue(0);
            entity.Property(e => e.State).HasDefaultValue(1);

            entity.HasOne(d => d.AddressNavigation).WithMany(p => p.PurchaseOrderDetails).HasConstraintName("fk_PurchaseOrderDetail_Address");

            entity.HasOne(d => d.PriceHistory).WithMany(p => p.PurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderDetail_PriceHistory");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseOrderDetail_PurchaseOrder");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC9C869718");

            entity.ToTable(tb => tb.HasComment("Quyền Người Dùng"));
        });

        modelBuilder.Entity<Shelve>(entity =>
        {
            entity.HasKey(e => e.ShelvesId).HasName("PK__Shelve__1FB01E56BA5CEFA4");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Shelves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shelve_Warehouse");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("PK__SubCateg__26BE5BF905256E2A");

            entity.ToTable("SubCategory", tb => tb.HasComment("Loại Sản Phẩm Phụ"));

            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Manufact__357E5CA107C94553");

            entity.ToTable("Supplier", tb => tb.HasComment("Nhà Sản Xuất"));

            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Suppliers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Manufacturer_Address");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.AccountName).HasName("PK__Users__406E0D2F043843EC");

            entity.ToTable(tb => tb.HasComment("Người Dùng"));

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_User_Roles");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.AccountName).HasName("PK__User_Inf__406E0D2F0AAF2A93");

            entity.ToTable("UserInfo", tb => tb.HasComment("Thông Tin Người Dùng"));

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.AccountNameNavigation).WithOne(p => p.UserInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_if_user");

            entity.HasOne(d => d.Address).WithMany(p => p.UserInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_if_Address");

            entity.HasOne(d => d.Customer).WithOne(p => p.UserInfo).HasConstraintName("fk_UserInfo_Customer");

            entity.HasOne(d => d.Employ).WithOne(p => p.UserInfo).HasConstraintName("fk_user_if_Employee");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("PK__Warehous__2608AFD944D38279");

            entity.ToTable("Warehouse", tb => tb.HasComment("Kho"));

            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeleteTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Address).WithMany(p => p.Warehouses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Warehouse_AddressID");
        });

        modelBuilder.Entity<WarehouseReceipt>(entity =>
        {
            entity.HasKey(e => e.WarehouseReceiptId).HasName("PK__Warehous__2DE0E1FAEFDF3134");

            entity.ToTable("WarehouseReceipt", tb => tb.HasComment("Phiếu Nhập Kho"));

            entity.Property(e => e.CreateAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.WarehouseReceipts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseReceipt_Employee");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseReceipts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_WarehouseReceipt_Warehouse");
        });

        modelBuilder.Entity<WarehouseReceiptDetail>(entity =>
        {
            entity.HasKey(e => new { e.WarehouseReceiptId, e.CellId }).HasName("pk_WarehouseReceiptDetail");

            entity.ToTable("WarehouseReceiptDetail", tb =>
                {
                    tb.HasComment("Chi Tiết Phiếu Nhập Kho");
                    tb.HasTrigger("TR_CheckWarehouseReceiptDetailCreateAt");
                    tb.HasTrigger("trg_UpdateCellOnWarehouseReceiptInsert");
                });

            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Cell).WithMany(p => p.WarehouseReceiptDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_WarehouseReceiptDetail_Cells");

            entity.HasOne(d => d.WarehouseReceipt).WithMany(p => p.WarehouseReceiptDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_WarehouseReceiptDetail_WarehouseReceipt");

            entity.HasOne(d => d.PurchaseOrderDetail).WithMany(p => p.WarehouseReceiptDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseReceiptDetail_PurchaseOrderDetail");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
