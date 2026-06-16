using ControleEstoque.API.DTOs;

namespace ControleEstoque.API.Services
{
    public interface IFormaPagamentoService
    {
        Task<IEnumerable<FormaPagamentoDto>> ObterTodosAsync();
        Task<FormaPagamentoDto?> ObterPorIdAsync(int id);
        Task<FormaPagamentoDto> CriarAsync(CriarFormaPagamentoDto dto);
        Task AtualizarAsync(AtualizarFormaPagamentoDto dto);
        Task RemoverAsync(int id);
    }
}