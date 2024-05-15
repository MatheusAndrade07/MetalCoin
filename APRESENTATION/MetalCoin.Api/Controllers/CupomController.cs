using MetalCoin.API.Controllers;
using MetalCoin.Domain.Interfaces;
using MetalCoin.Domain;
using Microsoft.AspNetCore.Mvc;
using MetalCoin.Entities;

namespace MetalCoin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CupomController : ControllerBase
    {
        private readonly ICupomRepository _cupomRepository;

        public CupomController(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupom>>> ListarTodos()
        {
            var cupons = await _cupomRepository.ObterTodos();
            return Ok(cupons);
        }

        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<Cupom>>> ListarAtivosDisponiveis()
        {
            var cupons = await _cupomRepository.ObterCuponsAtivosEDisponiveis();
            return Ok(cupons);
        }

        [HttpGet("indisponiveis")]
        public async Task<ActionResult<IEnumerable<Cupom>>> ListarIndisponiveis()
        {
            var cupons = await _cupomRepository.ObterCuponsIndisponiveis();
            return Ok(cupons);
        }

        [HttpPost]
        public async Task<ActionResult<Cupom>> Cadastrar([FromBody] Cupom cupom)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (cupom.DataValidade < DateTime.Now)
            {
                return BadRequest("Data de validade não pode estar no passado.");
            }

            await _cupomRepository.Adicionar(cupom);
            return CreatedAtAction(nameof(ListarTodos), new { id = cupom.Id }, cupom);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Cupom>> Atualizar(int id, [FromBody] Cupom cupom)
        {
            if (id != cupom.Id) return BadRequest("O ID do cupom informado na URL não corresponde ao ID do cupom no corpo da requisição.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (cupom.DataValidade < DateTime.Now)
            {
                return BadRequest("Data de validade não pode estar no passado.");
            }

            await _cupomRepository.Atualizar(cupom);
            return Ok(cupom);
        }

        [HttpPatch("{id:int}/ativar")]
        public async Task<ActionResult> Ativar(int id)
        {
            await _cupomRepository.AtivarCupom(id);
            return Ok();
        }

        [HttpPatch("{id:int}/desativar")]
        public async Task<ActionResult> Desativar(int id)
        {
            await _cupomRepository.DesativarCupom(id);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Excluir(int id)
        {
            var cupom = await _cupomRepository.ObterPorId(id);
            if (cupom == null) return NotFound("Cupom não encontrado.");

            if (await _cupomRepository.CupomJaFoiUtilizado(id))
            {
                return BadRequest("Existem pedidos utilizando este cupom. Operação de exclusão cancelada.");
            }

            await _cupomRepository.Remover(id);
            return Ok();
        }

        // 9. Usar Cupom
        [HttpPost("{id:int}/usar")]
        public async Task<ActionResult> UsarCupom(int id)
        {
            var cupom = await _cupomRepository.ObterPorId(id);
            if (cupom == null) return NotFound("Cupom não encontrado.");

            // Verificar se o cupom está ativo e disponível
            if (cupom.Status != StatusCupom.Ativo || cupom.QuantidadeUsada >= cupom.QuantidadeLiberada)
            {
                return BadRequest("Cupom não está disponível para uso.");
            }

            // Bloqueio de Cupons Expirados
            if (cupom.DataValidade < DateTime.Now)
            {
                return BadRequest("Cupom expirado.");
            }

            // Reduzir a quantidade disponível do cupom
            cupom.QuantidadeUsada++;
            await _cupomRepository.Atualizar(cupom);

            return Ok();
        }
    }
}