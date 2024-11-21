﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftCast.Models;
using SoftCast.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ConteudosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ConteudosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Conteudo>>> GetConteudos()
    {
        return await _context.Conteudos.Include(c => c.Criador).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Conteudo>> GetConteudo(int id)
    {
        var conteudo = await _context.Conteudos.Include(c => c.Criador)
                                               .FirstOrDefaultAsync(c => c.ID == id);
        if (conteudo == null) return NotFound();
        return conteudo;
    }

    [HttpPost]
    public async Task<ActionResult<Conteudo>> CreateConteudo(ConteudoNovo conteudoNovo)
    {
        // Valida se o ConteudoNovo é válido
        if (conteudoNovo == null)
        {
            return BadRequest("Conteúdo inválido.");
        }

        // Cria um objeto Conteudo com base no ConteudoNovo
        var conteudo = new Conteudo
        {
            Titulo = conteudoNovo.Titulo,
            Tipo = conteudoNovo.Tipo,
            Descricao = conteudoNovo.Descricao,
            ClassificacaoIndicativa = conteudoNovo.ClassificacaoIndicativa,
            VideoPath = conteudoNovo.VideoPath,
            CriadorID = conteudoNovo.CriadorID
        };

        // Adiciona ao contexto
        _context.Conteudos.Add(conteudo);
        await _context.SaveChangesAsync();

        // Retorna o recurso criado
        return CreatedAtAction(nameof(GetConteudo), new { id = conteudo.ID }, conteudo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConteudo(int id, Conteudo conteudo)
    {
        if (id != conteudo.ID) return BadRequest();
        _context.Entry(conteudo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConteudo(int id)
    {
        var conteudo = await _context.Conteudos.FindAsync(id);
        if (conteudo == null) return NotFound();
        _context.Conteudos.Remove(conteudo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
