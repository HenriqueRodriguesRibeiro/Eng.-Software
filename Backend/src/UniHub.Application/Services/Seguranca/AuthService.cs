using System;
using System.Threading.Tasks;
using Google.Apis.Auth;
using UniHub.Domain.Entities;

namespace UniHub.Application.Services;

public class AuthService
{
    public async Task<Usuario> ValidarLoginGoogleAsync(string idToken)
    {
        // Valida a assinatura do token diretamente com os servidores do Google
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

        // Bloqueia o acesso de qualquer e-mail que não seja da instituição
        if (!payload.Email.EndsWith("@unifesp.br"))
        {
            throw new UnauthorizedAccessException("Acesso negado. Utilize seu e-mail @unifesp.br.");
        }

        // Retorna o usuário validado (futuramente, conectaremos com o banco aqui)
        return new Usuario
        {
            EmailInstitucional = payload.Email,
            NomeCompleto = payload.Name,
            GoogleId = payload.Subject
        };
    }
}