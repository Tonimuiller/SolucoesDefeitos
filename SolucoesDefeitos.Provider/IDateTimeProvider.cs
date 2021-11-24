using System;

namespace SolucoesDefeitos.Provider
{
    public interface IDateTimeProvider
    {
        DateTime CurrentDateTime { get; }
    }
}