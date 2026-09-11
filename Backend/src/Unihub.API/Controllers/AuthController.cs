using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniHub.Application.DTOs;
using UniHub.Application.Services;

namespace UniHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController()
    {
        _authService = new AuthService();
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO request)
    {
        try
        {
            var usuarioValidado = await _authService.ValidarLoginGoogleAsync(request.IdToken);
            return Ok(new { Mensagem = "Login autorizado!", Usuario = usuarioValidado.NomeCompleto });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Erro = ex.Message });
        }
    }
}