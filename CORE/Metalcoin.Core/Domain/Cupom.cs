using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Metalcoin.Core.Enums;

namespace MetalCoin.Entities
{
    public class Cupom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Codigo { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        [Required]
        public decimal ValorDesconto { get; set; }

        [Required]
        public TipoDesconto TipoDesconto { get; set; }

        [Required]
        public DateTime DataValidade { get; set; }

        [Required]
        public int QuantidadeLiberada { get; set; }

        [Required]
        public int QuantidadeUsada { get; set; }

        [Required]
        public StatusCupom Status { get; set; }
    }
    
}