using System;

namespace UniHub.Domain.Entities;

/// Produto do modulo Alimentacao. Tem urgencia de tempo (horario de
/// retirada) que ItemBazar nao tem -- por isso esse campo fica aqui
/// e nao na classe base Produto.
public class Alimento : Produto
{
    public string Categoria { get; set; } = string.Empty; // ex: "Salgado", "Doce", "Bebida"

    public string? Restricoes { get; set; } // ex: "Contem gluten", "Vegano"

    // horario ate quando o pedido pode ser retirado. A validacao de que
    // uma reserva nao pode ser feita fora desse horario fica na camada
    // de Service, nao aqui.
    public TimeSpan HorarioDisponibilidade { get; set; }

    public override string GetTipo() => "Alimento";
}
