using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftCast.Models;
using SoftCast.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    // Endpoint de registro de usuário
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUsuario([FromBody] UsuarioRegister usuarioRegister)
    {
        if (usuarioRegister == null || string.IsNullOrEmpty(usuarioRegister.Email) || string.IsNullOrEmpty(usuarioRegister.Senha))
        {
            return BadRequest("Dados de cadastro inválidos.");
        }

        // Verificar se o usuário já existe
        var existingUsuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == usuarioRegister.Email);

        if (existingUsuario != null)
        {
            return BadRequest("Já existe um usuário com esse e-mail.");
        }

        // Criar o usuário
        var usuario = new Usuario
        {
            Nome = usuarioRegister.Nome,
            Email = usuarioRegister.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(usuarioRegister.Senha),
            DtNascimento = usuarioRegister.DtNascimento
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Cadastro realizado com sucesso", UsuarioId = usuario.ID });
    }

    // Endpoint de login de usuário
    [HttpPost("Login")]
    public IActionResult Login([FromBody] UsuarioLogin loginRequest)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Senha))
        {
            return BadRequest(new { Message = "Email e senha são obrigatórios." });
        }

        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == loginRequest.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Senha, usuario.Senha))
        {
            return Unauthorized(new { Message = "Credenciais inválidas." });
        }

        return Ok(new
        {
            Message = "Login realizado com sucesso!",
            Usuario = new
            {
                usuario.ID,
                usuario.Nome,
                usuario.Email
            }
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.Include(u => u.Playlists).ToListAsync();
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<Usuario>> GetUsuarios(string email)
    {
        var usuario = await _context.Usuarios.Include(u => u.Playlists)
                                              .FirstOrDefaultAsync(u => u.Email == email);
        if (usuario == null) return NotFound();
        return usuario;
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.Include(u => u.Playlists)
                                             .FirstOrDefaultAsync(u => u.ID == id);
        if (usuario == null) return NotFound();
        return usuario;
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> CreateUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.ID }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
    {
        if (id != usuario.ID) return BadRequest();
        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}