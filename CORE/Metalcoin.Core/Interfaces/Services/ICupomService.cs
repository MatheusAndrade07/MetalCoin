using Metalcoin.Core.Domain;
using MetalCoin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Metalcoin.Core.Dtos.Categorias;
using Metalcoin.Core.Dtos.Request;
using Metalcoin.Core.Dtos.Response;

namespace Metalcoin.Core.Interfaces.Services
{
    public interface ICupomService
    {
        Task<Cupom> CriarCupom(Cupom cupom);
        Task<Cupom> AplicarCupom(int cupomId, int pedidoId);
        Task<Cupom> ObterCupomPorCodigo(string codigo);
        Task<IEnumerable<Cupom>> ObterCuponsAtivos();
        Task<bool> ValidarCupom(int cupomId);
        Task AtivarCupom(int cupomId);
        Task DesativarCupom(int cupomId);
    }
}