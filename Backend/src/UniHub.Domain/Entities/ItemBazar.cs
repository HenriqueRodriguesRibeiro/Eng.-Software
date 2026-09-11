using System;

namespace UniHub.Domain.Entities;

public enum CondicaoItem
{
    Novo = 0,
    SeminNovo = 1,
    Usado = 2
}

/// Produto do modulo Bazar Academico (livros, calculadoras, equipamentos).
/// Diferente de Alimento, nao tem urgencia de horario -- a transacao eh
/// combinada diretamente entre comprador e vendedor.
public class ItemBazar : Produto
{
    public string Categoria { get; set; } = string.Empty; // ex: "Livro", "Calculadora", "Equipamento de laboratorio"

    public CondicaoItem Condicao { get; set; } = CondicaoItem.Usado;

    public override string GetTipo() => "ItemBazar";
}
