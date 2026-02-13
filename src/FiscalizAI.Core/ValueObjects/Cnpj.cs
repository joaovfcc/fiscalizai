namespace FiscalizAI.Core.Domain.ValueObjects;

public readonly record struct Cnpj
{
    private static readonly int[] Multiplicadores1 = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] Multiplicadores2 = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
    private readonly string _value;

    public Cnpj(string value)
    {
        var limpo = LimparFormatacao(value);
        if (!ValidarLogica(limpo))
            throw new ArgumentException("CNPJ inválido.", nameof(value));

        _value = limpo;
    }

    public string Value => _value ?? string.Empty;

    public static bool Validar(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        var limpo = LimparFormatacao(cnpj);
        return ValidarLogica(limpo);
    }

    private static bool ValidarLogica(string cnpjLimpo)
    {
        if (cnpjLimpo.Length != 14)
            return false;

        if (cnpjLimpo.All(c => c == cnpjLimpo[0]))
            return false;

        var baseCalculo = cnpjLimpo[..12];
        var soma = 0;

        for (int i = 0; i < 12; i++)
            soma += (baseCalculo[i] - '0') * Multiplicadores1[i];

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        baseCalculo += digito1;
        soma = 0;

        for (int j = 0; j < 13; j++)
            soma += (baseCalculo[j] - '0') * Multiplicadores2[j];

        var resto2 = soma % 11;
        var digito2 = resto2 < 2 ? 0 : 11 - resto2;

        return cnpjLimpo.EndsWith($"{digito1}{digito2}");
    }

    private static string LimparFormatacao(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return string.Empty;

        return new string(cnpj.Where(char.IsDigit).ToArray());
    }
    public override string ToString() => Value;
    
    public string Formatado() =>
        _value.Length == 14
            ? Convert.ToUInt64(_value).ToString(@"00\.000\.000\/0000\-00")
            : Value;
    
    public static implicit operator Cnpj(string value) => new Cnpj(value);
}