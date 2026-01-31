using Axia.Veiculos.Domain.Enums;

namespace Axia.Veiculos.Domain.Entities;

public class Veiculo
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public Marca Marca { get; private set; }
    public string Modelo { get; private set; } = string.Empty;
    public string? Opcionais { get; private set; }
    public decimal? Valor { get; private set; }

    protected Veiculo() { }

    public static Veiculo Create(string descricao, Marca marca, string modelo, string? opcionais = null, decimal? valor = null)
    {
        return new Veiculo
        {
            Id = Guid.NewGuid(),
            Descricao = descricao,
            Marca = marca,
            Modelo = modelo,
            Opcionais = opcionais,
            Valor = valor
        };
    }

    public void Update(string descricao, Marca marca, string modelo, string? opcionais, decimal? valor)
    {
        Descricao = descricao;
        Marca = marca;
        Modelo = modelo;
        Opcionais = opcionais;
        Valor = valor;
    }
}
