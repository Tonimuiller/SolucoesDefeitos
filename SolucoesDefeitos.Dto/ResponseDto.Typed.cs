namespace SolucoesDefeitos.Dto
{
    public class ResponseDto<T> : ResponseDto
        where T : class
    {
        private readonly T content;

        public ResponseDto(bool success, T content = null, params string[] errors) 
            : base(success, errors)
        {
            this.content = content;
        }

        public ResponseDto(params string[] errors)
            :base(false, errors)
        {
        }

        public T Content => content;
    }
}