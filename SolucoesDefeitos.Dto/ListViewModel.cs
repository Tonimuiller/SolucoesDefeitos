using System;
using System.Collections.Generic;

namespace SolucoesDefeitos.Dto
{
    public class ListViewModel<TData>
        where TData : class
    {
        private int _pageSize;

        public ListViewModel()
        {
            PageSize = 10;
        }

        public IEnumerable<TData> Data { get; set;}
        public int TotalRecords { get; set; }
        public int TotalPages 
        {
            get => (int) Math.Ceiling((decimal) TotalRecords / PageSize);
        }
        public int CurrentPage { get; set; }
        public int PageSize 
        { 
            get => _pageSize; 
            set
            {
                _pageSize = (value > 0) ? value : 10; 
            }
        }

    }
}
