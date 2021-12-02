using System.Collections.Generic;

namespace SolucoesDefeitos.Dto
{
    public class ResponseDto
    {
        private readonly bool success;
        private readonly IEnumerable<string> errors;

        public ResponseDto(bool success, params string[] errors)
        {
            this.success = success;
            this.errors = errors;
        }

        public bool Success => success;

        public IEnumerable<string> Errors => errors;
    }
}
