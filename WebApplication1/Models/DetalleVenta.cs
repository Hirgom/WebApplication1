using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class DetalleVenta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Venta")]
        public int VentaId { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        // Propiedad calculada (se genera automáticamente en BD)
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; private set; }

        // Relaciones
        [ForeignKey("VentaId")]
        public virtual Venta? Venta { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Product? Producto { get; set; }
    }
}