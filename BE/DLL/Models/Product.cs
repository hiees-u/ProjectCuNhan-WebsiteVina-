using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DLL.Models;

/// <summary>
/// Sản Phẩm
/// </summary>
[Table("Product")]
[Index("ProductName", Name = "uni_product_name", IsUnique = true)]
public partial class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("product_name")]
    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column("image")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Image { get; set; }

    [Column("totalQuantity")]
    public int? TotalQuantity { get; set; }

    [Column("Category_id")]
    public int CategoryId { get; set; }

    public int Supplier { get; set; }

    [Column("SubCategoryID")]
    public int SubCategoryId { get; set; }

    public DateTime? ExpriryDate { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    [StringLength(25)]
    [Unicode(false)]
    public string? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeleteTime { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<Cell> Cells { get; set; } = new List<Cell>();

    [InverseProperty("Product")]
    public virtual ICollection<DeliveryNoteDetail> DeliveryNoteDetails { get; set; } = new List<DeliveryNoteDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();

    [ForeignKey("SubCategoryId")]
    [InverseProperty("Products")]
    public virtual SubCategory SubCategory { get; set; } = null!;

    [ForeignKey("Supplier")]
    [InverseProperty("Products")]
    public virtual Supplier SupplierNavigation { get; set; } = null!;
}
