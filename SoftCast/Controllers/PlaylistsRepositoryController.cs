﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftCast.Models;
using SoftCast.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PlaylistsRepositoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public PlaylistsRepositoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
    {
        return await _context.Playlists.Include(p => p.Conteudos).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Playlist>> GetPlaylist(int id)
    {
        var playlist = await _context.Playlists.Include(p => p.Conteudos)
                                               .FirstOrDefaultAsync(p => p.ID == id);
        if (playlist == null) return NotFound();
        return playlist;
    }

    [HttpGet("usuario/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylistsByUserId(int usuarioId)
    {
        var playlists = await _context.Playlists
                                      .Where(p => p.UsuarioID == usuarioId)  // Filtra pelas playlists do usuário
                                      .Include(p => p.Conteudos)
                                      .ToListAsync();

        if (playlists == null || !playlists.Any())
        {
            return NotFound(new { Message = "Nenhuma playlist encontrada para este usuário." });
        }

        return Ok(playlists);  // Retorna as playlists encontradas
    }


    [HttpPost]
    public async Task<ActionResult<Playlist>> CreatePlaylist(PlaylistEdit playlistNova)
    {
        // Valida se o ConteudoNovo é válido
        if (playlistNova == null)
        {
            return BadRequest("Conteúdo inválido.");
        }

        // Cria um objeto Conteudo com base no ConteudoNovo
        var playlist = new Playlist
        {
            Nome = playlistNova.Nome,
            UsuarioID = playlistNova.UsuarioID
        };

        // Adiciona ao contexto
        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();

        // Retorna o recurso criado
        return CreatedAtAction(nameof(GetPlaylist), new { id = playlist.ID }, playlist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlaylist(int id, PlaylistEdit playlist)
    {
        if (id != playlist.ID) return BadRequest();
        
        var editPlaylist = await _context.Playlists.FindAsync(id);
        if (editPlaylist == null) return NotFound();

        // Atualiza propriedades
        editPlaylist.Nome = playlist.Nome;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var playlist = await _context.Playlists.FindAsync(id);
        if (playlist == null) return NotFound();
        _context.Playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
