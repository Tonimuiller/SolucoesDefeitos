namespace SolucoesDefeitos.Dto.Anomaly
{
    public class UpdateAnomalyResponseDto : ResponseDto
    {
        private readonly bool anomlyNotFound;

        public UpdateAnomalyResponseDto(bool success, bool anomlyNotFound = false, params string[] errors) 
            : base(success, errors)
        {
            this.anomlyNotFound = anomlyNotFound;
        }

        public bool AnomlyNotFound => anomlyNotFound;
    }
}
