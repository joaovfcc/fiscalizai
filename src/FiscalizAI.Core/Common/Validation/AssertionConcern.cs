using System.Text.RegularExpressions;
using FiscalizAI.Core.Domain.Exceptions;

namespace FiscalizAI.Core.Common.Validation;

public class AssertionConcern
{
    public static void AssertArgumentTrue(bool boolValue, string message)
    {
        if (!boolValue) throw new DomainException(message);
    }

    // Valida string vazia ou nula
    public static void AssertArgumentNotEmpty(string stringValue, string message)
    {
        if (string.IsNullOrWhiteSpace(stringValue)) throw new DomainException(message);
    }

    // Valida tamanho de string (Min/Max)
    public static void AssertArgumentLength(string stringValue, int maximum, string message)
    {
        var length = stringValue.Trim().Length;
        if (length > maximum) throw new DomainException(message);
    }

    public static void AssertArgumentLength(string stringValue, int minimum, int maximum, string message)
    {
        var length = stringValue.Trim().Length;
        if (length < minimum || length > maximum) throw new DomainException(message);
    }

    // Valida nulos (para Objetos Obrigatórios)
    public static void AssertArgumentNotNull(object object1, string message)
    {
        if (object1 == null) throw new DomainException(message);
    }
    
    // Valida RegEx (Útil para Email, CNPJ se fosse string, etc)
    public static void AssertArgumentMatches(string pattern, string stringValue, string message)
    {
        var regex = new Regex(pattern);
        if (!regex.IsMatch(stringValue)) throw new DomainException(message);
    }

    internal static void AssertArgumentGreaterThan(decimal valorTotal, int v1, string v2)
    {
      if (valorTotal <= v1) throw new DomainException(v2);
    }
}