using MetalCoin.Domain;
using MetalCoin.Domain.Interfaces;
using MetalCoin.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetalCoin.Application.Services
{
    public class CupomService
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
    }
}