using System;

namespace UniHub.Domain.Entities;

public class ProdutoBazar
{
    // Identificador único alinhado com o padrão Guid do time
    public Guid Id { get; set; } = Guid.NewGuid();

    // Informações do item do Bazar
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty; // Ex: Livros, Jalecos, Calculadoras
    public bool Disponivel { get; set; } = true;
    public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;

    // Chave estrangeira ligando ao Vendedor da equipe
    public Guid VendedorId { get; set; }

    // Propriedade de navegação do Entity Framework Core
    public Vendedor? Vendedor { get; set; }
}