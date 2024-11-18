using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftCast.Models;
using SoftCast.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ItensPlaylistController : ControllerBase
{
    private readonly AppDbContext _context;

    public ItensPlaylistController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemPlaylist>>> GetItensPlaylist()
    {
        return await _context.ItensPlaylist.Include(ip => ip.Playlist)
                                           .Include(ip => ip.Conteudo)
                                           .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<ItemPlaylist>> CreateItemPlaylist(ItemPlaylist item)
    {
        _context.ItensPlaylist.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetItensPlaylist), new { item.PlaylistID, item.ConteudoID }, item);
    }

    [HttpDelete("{playlistId}/{conteudoId}")]
    public async Task<IActionResult> DeleteItemPlaylist(int playlistId, int conteudoId)
    {
        var item = await _context.ItensPlaylist.FindAsync(playlistId, conteudoId);
        if (item == null) return NotFound();
        _context.ItensPlaylist.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
