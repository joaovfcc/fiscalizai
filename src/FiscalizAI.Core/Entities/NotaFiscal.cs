using FiscalizAI.Core.Common.Bases;
using FiscalizAI.Core.Common.Interfaces;
using FiscalizAI.Core.Common.Validation;
using FiscalizAI.Core.Enums;
using FiscalizAI.Core.Domain.ValueObjects;
using FiscalizAI.Core.Entities;

public class NotaFiscal : BaseEntity, IAggregateRoot
    {
    public Guid EmpresaId { get; private set; }

    // Índice Único (Buscas rápidas)
    public string ChaveAcesso { get; private set; }

    // Metadados para Relatórios/Listagem (Promoted Fields)
    public long Numero { get; private set; }
    public int Serie { get; private set; }
    public Cnpj CnpjEmitente { get; private set; }
    public string NomeEmitente { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public decimal ValorTotal { get; private set; }
    public StatusNota Status { get; private set; }
    public TipoOperacao Tipo { get; private set; }
    public Empresa Empresa { get; private set; }
    public string CaminhoStorage { get; private set; }

    private NotaFiscal()
    {
    }

    public NotaFiscal(Guid empresaId, string chaveAcesso, long numero, int serie, Cnpj cnpjEmitente, string nomeEmitente, DateTime dataEmissao, 
                      decimal valorTotal, StatusNota status, TipoOperacao tipo, string caminhoStorage)
    {
        EmpresaId = empresaId;
        ChaveAcesso = chaveAcesso;
        Numero = numero;
        Serie = serie;
        CnpjEmitente = cnpjEmitente;
        NomeEmitente = nomeEmitente;
        DataEmissao = dataEmissao;
        ValorTotal = valorTotal;
        Status = status;
        Tipo = tipo;
        CaminhoStorage = caminhoStorage;
        Validate();
       
    }

    private void Validate()
    {
        AssertionConcern.AssertArgumentNotEmpty(ChaveAcesso, "A Chave de Acesso é obrigatória.");
        AssertionConcern.AssertArgumentNotNull(CnpjEmitente, "O CNPJ do Emitente é obrigatório.");
        AssertionConcern.AssertArgumentNotEmpty(NomeEmitente, "O Nome do Emitente é obrigatório.");
        AssertionConcern.AssertArgumentGreaterThan(ValorTotal, 0, "O Valor Total deve ser maior que zero.");
        AssertionConcern.AssertArgumentTrue(Enum.IsDefined(typeof(StatusNota), Status), "Status da Nota Fiscal inválido.");
        AssertionConcern.AssertArgumentTrue(Enum.IsDefined(typeof(TipoOperacao), Tipo), "Tipo de Operação inválido.");
        AssertionConcern.AssertArgumentNotEmpty(CaminhoStorage, "O caminho do arquivo no storage é obrigatório.");
    }
}

