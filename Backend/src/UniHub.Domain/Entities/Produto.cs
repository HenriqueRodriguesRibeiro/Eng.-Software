using System;

namespace UniHub.Domain.Entities;

/// Classe base abstrata para qualquer produto publicado na plataforma
/// (Alimentacao ou Bazar Academico). Cada subtipo concreto (ex: ItemBazar,
/// Alimento) herda estes campos comuns e implementa GetTipo().
public abstract class Produto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public decimal Preco { get; set; }

    // chave estrangeira para o Vendedor dono deste produto.
    // usando Guid pra bater com Vendedor.Id (o diagrama escreve "int",
    // mas o resto do codigo do time usa Guid em todo lugar).
    public Guid VendedorId { get; set; }

    // propriedade de navegacao de volta para o Vendedor
    public Vendedor? Vendedor { get; set; }

    public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;

    // ex: "Disponivel", "Reservado", "Vendido"
    public string Status { get; set; } = "Disponivel";

    /// Cada subtipo concreto informa seu proprio tipo (ex: "ItemBazar", "Alimento"),
    /// usado pelo ProdutoService/Factory pra tratar produtos polimorficamente.
    public abstract string GetTipo();
}