using Metalcoin.Core.Enums;
using System;

namespace MetalCoin.Web.Spa.Core.Models
{
    public class CupomRequest
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public decimal ValorDesconto { get; set; }
        public TipoDesconto TipoDesconto { get; set; }
        public DateTime DataValidade { get; set; }
        public int QuantidadeLiberada { get; set; }
    }
}