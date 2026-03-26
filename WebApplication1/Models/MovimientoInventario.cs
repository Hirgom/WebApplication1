using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class MovimientoInventario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Tipo de Movimiento")]
        public string TipoMovimiento { get; set; } // "Entrada" o "Salida"

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Fecha del Movimiento")]
        [DataType(DataType.DateTime)]
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "Observación")]
        public string? Observacion { get; set; }

        // Propiedad calculada
        [NotMapped]
        [Display(Name = "Subtotal")]
        public decimal Subtotal => Cantidad * PrecioUnitario;

        // Relaciones
        [ForeignKey("ProductoId")]
        public virtual Product? Producto { get; set; }
    }
}