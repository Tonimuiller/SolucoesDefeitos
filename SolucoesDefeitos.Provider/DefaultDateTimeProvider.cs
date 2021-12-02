using System;

namespace SolucoesDefeitos.Provider
{
    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTime CurrentDateTime => DateTime.Now;
    }
}
