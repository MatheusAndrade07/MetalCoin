using MetalCoin.Domain;
using MetalCoin.Domain.Interfaces;
using MetalCoin.Entities;
using Metalcoin.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Metalcoin.Core.Interfaces.Services;

namespace MetalCoin.Application.Services
{
    public class CupomService : ICupomService
    {
        private readonly ICupomRepository _cupomRepository;

        public CupomService(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public async Task<Cupom> CriarCupom(Cupom cupom)
        {
            if (cupom.DataValidade < DateTime.Now)
            {
                throw new ArgumentException("Data de validade não pode estar no passado.");
            }

            await _cupomRepository.Adicionar(cupom);
            return cupom;
        }

        public async Task<Cupom> AplicarCupom(int cupomId, int pedidoId)
        {
            var cupom = await _cupomRepository.ObterPorId(cupomId);

            if (cupom == null || cupom.Status != StatusCupom.Ativo)
            {
                throw new ArgumentException("Cupom inválido ou inativo.");
            }

            if (cupom.DataValidade < DateTime.Now)
            {
                throw new ArgumentException("Cupom expirado.");
            }

            if (cupom.QuantidadeUsada >= cupom.QuantidadeLiberada)
            {
                throw new ArgumentException("Cupom já foi totalmente utilizado.");
            }

            
            cupom.QuantidadeUsada++;
            await _cupomRepository.Atualizar(cupom);

            return cupom;
        }

        public Task<Cupom> ObterCupomPorCodigo(string codigo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cupom>> ObterCuponsAtivos()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidarCupom(int cupomId)
        {
            throw new NotImplementedException();
        }

        public Task AtivarCupom(int cupomId)
        {
            throw new NotImplementedException();
        }

        public Task DesativarCupom(int cupomId)
        {
            throw new NotImplementedException();
        }
    }
}