using System;
using System.Collections.Generic;

namespace SolucoesDefeitos.Dto
{
    public class PagedData<TData>
        where TData : class
    {
        private int _pageSize = 10;
        private readonly int _pagingGroupSize = 3;

        public PagedData()
        {
            PageSize = 10;
        }

        public IEnumerable<TData> Data { get; set;} = new List<TData>();
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
        public bool SplitPaging => Math.Floor(TotalPages/(decimal)PagingGroupSize) >= 3;
        public int PagingGroupSize 
        { 
            get => _pagingGroupSize;
        }

        public bool CurrentPageIsNextBottomPagingGroup => CurrentPage == (PagingGroupSize + 1);
        public bool CurrentPageIsNextUpperPagingGroup => CurrentPage == (TotalPages - (PagingGroupSize + 1));

    }
}
