using Metalcoin.Core.Dtos;
using Metalcoin.Core.Dtos.Response;

namespace MetalCoin.Web.Spa.Core.Endpoint
{
    public class CupomEndpoint
    {
        private readonly HttpClient _httpClient;

        public CupomEndpoint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CupomResponse> Cadastrar(CupomRequest cupomRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<CupomResponse> ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CupomResponse>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public async Task<CupomResponse> Atualizar(int id, CupomRequest cupomRequest)
        {
            throw new NotImplementedException();
        }

        public async Task Excluir(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Ativar(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Desativar(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CupomResponse> Usar(int id)
        {
            throw new NotImplementedException();
        }
    }
}
