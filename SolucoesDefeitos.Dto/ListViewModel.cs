using System.Collections.Generic;

namespace SolucoesDefeitos.Dto
{
    public class ListViewModel<TData>
        where TData : class
    {
        public IEnumerable<TData> Data { get; set;}
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

    }
}
