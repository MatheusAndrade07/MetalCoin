using MetalCoin.Domain;
using MetalCoin.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetalCoin.Domain.Interfaces
{
    public interface ICupomRepository
    {
        Task<IEnumerable<Cupom>> ObterTodos();
        Task<Cupom> ObterPorId(int id);
        Task<IEnumerable<Cupom>> ObterCuponsAtivosEDisponiveis();
        Task<IEnumerable<Cupom>> ObterCuponsIndisponiveis();
        Task<bool> CupomJaFoiUtilizado(int id);
        Task Adicionar(Cupom cupom);
        Task Atualizar(Cupom cupom);
        Task Remover(int id);
        Task AtivarCupom(int id);
        Task DesativarCupom(int id);
    }
}