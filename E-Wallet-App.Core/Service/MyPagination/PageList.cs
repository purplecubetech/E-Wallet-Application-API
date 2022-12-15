using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace E_Wallet_App.Entity.Helper
{
    public class PageList< T> : List<T>
    {
        public Metadata metadata { get; set; }
        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            metadata = new Metadata
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalCount = count,
                TotalPage = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);

        }
        public static PageList<T> ToPageList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}