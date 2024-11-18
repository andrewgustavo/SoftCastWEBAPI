using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    public async Task<ActionResult<Playlist>> CreatePlaylist(Playlist playlist)
    {
        _context.Playlists.Add(playlist);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPlaylist), new { id = playlist.ID }, playlist);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlaylist(int id, Playlist playlist)
    {
        if (id != playlist.ID) return BadRequest();
        _context.Entry(playlist).State = EntityState.Modified;
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
