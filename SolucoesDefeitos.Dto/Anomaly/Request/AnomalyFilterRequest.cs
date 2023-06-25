using System.Linq;

namespace SolucoesDefeitos.Dto.Anomaly.Request
{
    public sealed class AnomalyFilterRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SearchTerm { get; set; }
        public int[] ManufacturerIds { get; set; }
        public int[] ProductGroupIds { get; set; }
        public int[] ProductIds { get; set; }
        public bool Filtered => !string.IsNullOrEmpty(SearchTerm)
            || (ManufacturerIds?.Any() ?? false)
            || (ProductGroupIds?.Any() ?? false)
            || (ProductIds?.Any() ?? false);
    }
}
