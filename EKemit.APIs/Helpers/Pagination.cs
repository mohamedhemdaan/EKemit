using EKemit.APIs.DTOs;

namespace EKemit.APIs.Helpers
{
    public class Pagination<T>
    {

        public Pagination(int pageSize, int pageindex, IReadOnlyList<T> data,int count)
        {
            PageSize = pageSize;
            PageIndex = pageindex;
            Data = data;
            Count = count;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        
    }
}
