using MetalCoin.API.Controllers;
using MetalCoin.Domain.Interfaces;
using Metalcoin.Core.Dtos;
using Metalcoin.Core.Dtos.Request;
using Metalcoin.Core.Dtos.Response;
using Microsoft.AspNetCore.Mvc;
using Metalcoin.Core.Enums;
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

        // GET: api/Cupom/todos (todos os cupons)
        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<CupomResponse>>> ObterTodosCupons()
        {
            try
            {
                var cupons = await _cupomRepository.ObterTodos();

                // Mapear entidades Cupom para CupomResponse
                var cupomResponses = cupons.Select(c => new CupomResponse
                {
                    Id = c.Id,
                    Codigo = c.Codigo,
                    Descricao = c.Descricao,
                    ValorDesconto = c.ValorDesconto,
                    TipoDesconto = c.TipoDesconto,
                    DataValidade = c.DataValidade,
                    QuantidadeLiberada = c.QuantidadeLiberada,
                    QuantidadeUsada = c.QuantidadeUsada,
                    Status = c.Status
                });

                return Ok(cupomResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor ao obter os cupons.");
            }
        }

        // GET: api/Cupom/ativos (apenas cupons ativos)
        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<CupomResponse>>> ObterCuponsAtivos()
        {
            try
            {
                var cupons = await _cupomRepository.ObterCuponsAtivosEDisponiveis();

                // Mapear entidades Cupom para CupomResponse
                var cupomResponses = cupons.Select(c => new CupomResponse
                {
                    Id = c.Id,
                    Codigo = c.Codigo,
                    Descricao = c.Descricao,
                    ValorDesconto = c.ValorDesconto,
                    TipoDesconto = c.TipoDesconto,
                    DataValidade = c.DataValidade,
                    QuantidadeLiberada = c.QuantidadeLiberada,
                    QuantidadeUsada = c.QuantidadeUsada,
                    Status = c.Status
                });

                return Ok(cupomResponses);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, "Erro interno do servidor ao obter os cupons ativos.");
            }
        }

        // GET: api/Cupom/indisponiveis
        [HttpGet("indisponiveis")]
        public async Task<ActionResult<IEnumerable<CupomResponse>>> ObterCuponsIndisponiveis()
        {
            try
            {
                var cupons = await _cupomRepository.ObterCuponsIndisponiveis();

                // Mapear entidades Cupom para CupomResponse
                var cupomResponses = cupons.Select(c => new CupomResponse
                {
                    Id = c.Id,
                    Codigo = c.Codigo,
                    Descricao = c.Descricao,
                    ValorDesconto = c.ValorDesconto,
                    TipoDesconto = c.TipoDesconto,
                    DataValidade = c.DataValidade,
                    QuantidadeLiberada = c.QuantidadeLiberada,
                    QuantidadeUsada = c.QuantidadeUsada,
                    Status = c.Status
                });

                return Ok(cupomResponses);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, "Erro interno do servidor ao obter os cupons indisponíveis.");
            }
        }

        // POST: api/Cupom/criar (cadastrar um novo cupom)
        [HttpPost("criar")]
        public async Task<ActionResult<CupomResponse>> CadastrarCupom(CupomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Mapear CupomRequest para Cupom
            var cupom = new Cupom
            {
                Codigo = request.Codigo,
                Descricao = request.Descricao,
                ValorDesconto = request.ValorDesconto,
                TipoDesconto = request.TipoDesconto,
                DataValidade = request.DataValidade,
                QuantidadeLiberada = request.QuantidadeLiberada,
                QuantidadeUsada = 0, // Inicializa como 0
                Status = StatusCupom.Ativo // Inicializa como Ativo
            };

            if (cupom.DataValidade < DateTime.Now)
            {
                return BadRequest("Data de validade não pode estar no passado.");
            }

            try
            {
                await _cupomRepository.Adicionar(cupom);
            }
            catch (Exception ex)
            {               
                return StatusCode(500, "Erro interno do servidor ao cadastrar o cupom.");
            }

            // Mapear Cupom para CupomResponse
            var response = new CupomResponse
            {
                Id = cupom.Id,
                Codigo = cupom.Codigo,
                Descricao = cupom.Descricao,
                ValorDesconto = cupom.ValorDesconto,
                TipoDesconto = cupom.TipoDesconto,
                DataValidade = cupom.DataValidade,
                QuantidadeLiberada = cupom.QuantidadeLiberada,
                QuantidadeUsada = cupom.QuantidadeUsada,
                Status = cupom.Status
            };

            return CreatedAtAction(nameof(ObterCupomPorId), new { id = cupom.Id }, response);
        }

        // PUT: api/Cupom/atualizar/{id} (atualizar um cupom existente)
        [HttpPut("atualizar/{id:int}")]
        public async Task<ActionResult<CupomResponse>> AtualizarCupom(int id, CupomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Obter o cupom existente pelo ID
            var cupomExistente = await _cupomRepository.ObterPorId(id);
            if (cupomExistente == null) return NotFound("Cupom não encontrado.");

            // Mapear CupomRequest para Cupom (atualizando o cupomExistente)
            cupomExistente.Codigo = request.Codigo;
            cupomExistente.Descricao = request.Descricao;
            cupomExistente.ValorDesconto = request.ValorDesconto;
            cupomExistente.TipoDesconto = request.TipoDesconto;
            cupomExistente.DataValidade = request.DataValidade;
            cupomExistente.QuantidadeLiberada = request.QuantidadeLiberada;
            // QuantidadeUsada e Status não devem ser atualizados via request

            if (cupomExistente.DataValidade < DateTime.Now)
            {
                return BadRequest("Data de validade não pode estar no passado.");
            }

            try
            {
                await _cupomRepository.Atualizar(cupomExistente);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, "Erro interno do servidor ao atualizar o cupom.");
            }

            // Mapear Cupom para CupomResponse
            var response = new CupomResponse
            {
                Id = cupomExistente.Id,
                Codigo = cupomExistente.Codigo,
                Descricao = cupomExistente.Descricao,
                ValorDesconto = cupomExistente.ValorDesconto,
                TipoDesconto = cupomExistente.TipoDesconto,
                DataValidade = cupomExistente.DataValidade,
                QuantidadeLiberada = cupomExistente.QuantidadeLiberada,
                QuantidadeUsada = cupomExistente.QuantidadeUsada,
                Status = cupomExistente.Status
            };

            return Ok(response);
        }

        // PATCH: api/Cupom/ativar/{id} 
        [HttpPatch("ativar/{id:int}")]
        public async Task<ActionResult> AtivarCupom(int id)
        {
            try
            {
                await _cupomRepository.AtivarCupom(id);
                return Ok();
            }
            catch (Exception ex)
            {                
                return StatusCode(500, "Erro interno do servidor ao ativar o cupom.");
            }
        }

        // PATCH: api/Cupom/desativar/{id}
        [HttpPatch("desativar/{id:int}")]
        public async Task<ActionResult> DesativarCupom(int id)
        {
            try
            {
                await _cupomRepository.DesativarCupom(id);
                return Ok();
            }
            catch (Exception ex)
            {               
                return StatusCode(500, "Erro interno do servidor ao desativar o cupom.");
            }
        }

        // DELETE: api/Cupom/excluir/{id} 
        [HttpDelete("excluir/{id:int}")]
        public async Task<ActionResult> ExcluirCupom(int id)
        {
            var cupom = await _cupomRepository.ObterPorId(id);
            if (cupom == null) return NotFound("Cupom não encontrado.");

            if (await _cupomRepository.CupomJaFoiUtilizado(id))
            {
                return BadRequest("Existem pedidos utilizando este cupom. Operação de exclusão cancelada.");
            }

            try
            {
                await _cupomRepository.Remover(id);
                return Ok();
            }
            catch (Exception ex)
            {              
                return StatusCode(500, "Erro interno do servidor ao excluir o cupom.");
            }
        }

        // POST: api/Cupom/usar/{id} 
        [HttpPost("usar/{id:int}")]
        public async Task<ActionResult> UsarCupom(int id)
        {
            var cupom = await _cupomRepository.ObterPorId(id);
            if (cupom == null) return NotFound("Cupom não encontrado.");

            if (cupom.Status != StatusCupom.Ativo || cupom.QuantidadeUsada >= cupom.QuantidadeLiberada)
            {
                return BadRequest("Cupom não está disponível para uso.");
            }

            if (cupom.DataValidade < DateTime.Now)
            {
                return BadRequest("Cupom expirado.");
            }

            try
            {
                cupom.QuantidadeUsada++;
                await _cupomRepository.Atualizar(cupom);
                return Ok();
            }
            catch (Exception ex)
            {               
                return StatusCode(500, "Erro interno do servidor ao usar o cupom.");
            }
        }

        // GET: api/Cupom/{id} (obter um cupom por ID)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CupomResponse>> ObterCupomPorId(int id)
        {
            try
            {
                var cupom = await _cupomRepository.ObterPorId(id);
                if (cupom == null) return NotFound("Cupom não encontrado.");

                // Mapear Cupom para CupomResponse
                var response = new CupomResponse
                {
                    Id = cupom.Id,
                    Codigo = cupom.Codigo,
                    Descricao = cupom.Descricao,
                    ValorDesconto = cupom.ValorDesconto,
                    TipoDesconto = cupom.TipoDesconto,
                    DataValidade = cupom.DataValidade,
                    QuantidadeLiberada = cupom.QuantidadeLiberada,
                    QuantidadeUsada = cupom.QuantidadeUsada,
                    Status = cupom.Status
                };

                return Ok(response);
            }
            catch (Exception ex)
            {               
                return StatusCode(500, "Erro interno do servidor ao obter o cupom.");
            }
        }
    }
}