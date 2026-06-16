using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.API.Services
{
    public class FormaPagamentoService : IFormaPagamentoService
    {
        private readonly AppDbContext _context;

        public FormaPagamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FormaPagamentoDto>> ObterTodosAsync()
        {
            return await _context.FormasPagamento
                .AsNoTracking()
                .Select(f => new FormaPagamentoDto
                {
                    Id = f.Id,
                    Nome = f.Nome,
                    Tipo = f.Tipo,
                    Ativo = f.Ativo
                })
                .ToListAsync();
        }

        public async Task<FormaPagamentoDto?> ObterPorIdAsync(int id)
        {
            var forma = await _context.FormasPagamento
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (forma == null) return null;

            return new FormaPagamentoDto
            {
                Id = forma.Id,
                Nome = forma.Nome,
                Tipo = forma.Tipo,
                Ativo = forma.Ativo
            };
        }

        public async Task<FormaPagamentoDto> CriarAsync(CriarFormaPagamentoDto dto)
        {
            var forma = new FormaPagamento
            {
                Nome = dto.Nome,
                Tipo = dto.Tipo,
                Ativo = dto.Ativo
            };

            _context.FormasPagamento.Add(forma);
            await _context.SaveChangesAsync();

            return new FormaPagamentoDto
            {
                Id = forma.Id,
                Nome = forma.Nome,
                Tipo = forma.Tipo,
                Ativo = forma.Ativo
            };
        }

        public async Task AtualizarAsync(AtualizarFormaPagamentoDto dto)
        {
            var forma = await _context.FormasPagamento.FindAsync(dto.Id);
            if (forma != null)
            {
                forma.Nome = dto.Nome;
                forma.Tipo = dto.Tipo;
                forma.Ativo = dto.Ativo;

                _context.FormasPagamento.Update(forma);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoverAsync(int id)
        {
            var forma = await _context.FormasPagamento.FindAsync(id);
            if (forma != null)
            {
                _context.FormasPagamento.Remove(forma);
                await _context.SaveChangesAsync();
            }
        }
    }
}