using System;

namespace UniHub.Domain.Entities;

// Esqueleto criado temporariamente para que Vendedor.cs possa compilar.
// A implementação real será feita pela equipe do Bazar Acadêmico.
public class Produto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Adicione apenas propriedades mínimas se necessário para parar os erros
    public string Nome { get; set; } = string.Empty;
}