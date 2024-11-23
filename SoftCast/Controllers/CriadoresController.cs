using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftCast.Models;
using SoftCast.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]
public class CriadoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public CriadoresController(AppDbContext context)
    {
        _context = context;
    }
    // Endpoint de cadastro
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterCriador([FromBody] CriadorRegister CriadorRegister)
    {
        if (CriadorRegister == null || string.IsNullOrEmpty(CriadorRegister.Email) || string.IsNullOrEmpty(CriadorRegister.Senha))
        {
            return BadRequest("Dados de cadastro inválidos.");
        }

        // Verificar se o Criador já existe
        var existingCriador = await _context.Criadores
            .FirstOrDefaultAsync(c => c.Email == CriadorRegister.Email);

        if (existingCriador != null)
        {
            return BadRequest("Já existe um criador com esse e-mail.");
        }

        // Criar o Criador
        var criador = new Criador
        {
            Nome = CriadorRegister.Nome,
            Email = CriadorRegister.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(CriadorRegister.Senha) 
        };

        _context.Criadores.Add(criador);
        await _context.SaveChangesAsync();

        // Retornar uma resposta indicando sucesso, sem obrigar a criação de lista de conteúdos
        return Ok(new { Message = "Cadastro realizado com sucesso", CriadorId = criador.ID });
    }

    // Endpoint de login 
    [HttpPost("Login")]
    public IActionResult Login([FromBody] CriadorLogin loginRequest)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Senha))
        {
            return BadRequest(new { Message = "Email e senha são obrigatórios." });
        }

        var criador = _context.Criadores.FirstOrDefault(c => c.Email == loginRequest.Email);

        if (criador == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Senha, criador.Senha))
        {
            return Unauthorized(new { Message = "Credenciais inválidas." });
        }

        return Ok(new
        {
            Message = "Login realizado com sucesso!",
            Criador = new
            {
                criador.ID,
                criador.Nome,
                criador.Email
            }
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Criador>>> GetCriadores()
    {
        return await _context.Criadores.Include(c => c.Conteudos).ToListAsync();
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<Criador>> GetCriador(string email)
    {
        var criador = await _context.Criadores.Include(c => c.Conteudos)
                                              .FirstOrDefaultAsync(c => c.Email == email);
        if (criador == null) return NotFound();
        return criador;
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<Criador>> GetCriadorById(int id)
    {
        var criador = await _context.Criadores.Include(c => c.Conteudos)
                                              .FirstOrDefaultAsync(c => c.ID == id);
        if (criador == null)
        {
            return NotFound(new { Message = "Criador não encontrado." });
        }
        return criador;
    }

    [HttpPost]
    public async Task<ActionResult<Criador>> CreateCriador(Criador criador)
    {
        _context.Criadores.Add(criador);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCriador), new { id = criador.ID }, criador);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCriador(int id, Criador criador)
    {
        if (id != criador.ID) return BadRequest();
        _context.Entry(criador).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{add-content}")]
    public async Task<IActionResult> AddContentToCriador(int criadorId, [FromBody] Conteudo conteudoDto)
    {
        var criador = await _context.Criadores.Include(c => c.Conteudos)
                                              .FirstOrDefaultAsync(c => c.ID == criadorId);

        if (criador == null)
        {
            return NotFound("Criador não encontrado.");
        }

        var conteudo = new Conteudo
        {
            Titulo = conteudoDto.Titulo,
            Descricao = conteudoDto.Descricao,
            CriadorID = criadorId,
            // Outros campos necessários
        };

        criador.Conteudos.Add(conteudo);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Conteúdo adicionado com sucesso." });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCriador(int id)
    {
        var criador = await _context.Criadores.FindAsync(id);
        if (criador == null) return NotFound();
        _context.Criadores.Remove(criador);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
