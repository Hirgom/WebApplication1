using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fecha de Venta")]
        [DataType(DataType.DateTime)]
        public DateTime FechaVenta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Cliente")]
        public string ClienteNombre { get; set; }

        [StringLength(20)]
        [Display(Name = "Documento")]
        public string? ClienteDocumento { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Pendiente";

        [StringLength(50)]
        [Display(Name = "Método de Pago")]
        public string? MetodoPago { get; set; }

        // Relación: Una venta tiene muchos detalles
        public virtual ICollection<DetalleVenta>? Detalles { get; set; }
    }
}