using Metalcoin.Core.Enums;
using Metalcoin.Core.Domain;
using MetalCoin.Domain.Interfaces;
using MetalCoin.Entities;
using MetalCoin.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetalCoin.Infra.Data.Repositories
{
    public class CupomRepository : ICupomRepository
    {
        private readonly AppDbContext _context;

        public CupomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cupom>> ObterTodos()
        {
            return await _context.Cupons.ToListAsync();
        }

        public async Task<Cupom> ObterPorId(int id)
        {
            return await _context.Cupons.FindAsync(id);
        }

        public async Task<IEnumerable<Cupom>> ObterCuponsAtivosEDisponiveis()
        {
            return await _context.Cupons
                .Where(c => c.Status == StatusCupom.Ativo && c.QuantidadeUsada < c.QuantidadeLiberada)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cupom>> ObterCuponsIndisponiveis()
        {
            return await _context.Cupons
                .Where(c => c.Status != StatusCupom.Ativo || c.QuantidadeUsada >= c.QuantidadeLiberada)
                .ToListAsync();
        }

        public async Task<bool> CupomJaFoiUtilizado(int id)
        {
            return await _context.Cupons.AnyAsync(c => c.Id == id && c.QuantidadeUsada > 0);
        }

        public async Task Adicionar(Cupom cupom)
        {
            _context.Cupons.Add(cupom);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Cupom cupom)
        {
            _context.Cupons.Update(cupom);
            await _context.SaveChangesAsync();
        }

        public async Task Remover(int id)
        {
            var cupom = await _context.Cupons.FindAsync(id);
            if (cupom != null)
            {
                _context.Cupons.Remove(cupom);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AtivarCupom(int id)
        {
            var cupom = await _context.Cupons.FindAsync(id);
            if (cupom != null)
            {
                cupom.Status = StatusCupom.Ativo;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DesativarCupom(int id)
        {
            var cupom = await _context.Cupons.FindAsync(id);
            if (cupom != null)
            {
                cupom.Status = StatusCupom.Desativado;
                await _context.SaveChangesAsync();
            }
        }
    }
}