using System;

namespace UniHub.Domain.Entities;

/// Representa um item a venda no Bazar Academico (livros, calculadoras,
/// equipamentos usados). Os dados basicos (Id, Nome, Descricao, Preco,
/// VendedorId, DataPublicacao, Status) vem herdados de Produto.
public class ProdutoBazar : Produto
{
    // categoria do item, ex: Livros, Calculadoras, Equipamentos de Laboratorio
    public string Categoria { get; set; } = string.Empty;

    // estado de conservacao do item, ex: Novo, Seminovo, Usado
    public string Condicao { get; set; } = string.Empty;

    // quantidade disponivel deste item (um vendedor pode ter mais de uma unidade)
    public int Quant { get; set; } = 1;

    public override string GetTipo()
    {
        return "ProdutoBazar";
    }
}