namespace SolucoesDefeitos.Dto.Anomaly.Request
{
    public sealed class AnomalyFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SearchTerm { get; set; }
    }
}
