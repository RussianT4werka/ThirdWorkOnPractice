using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ООО__Товары_для_животных_.Database;
using ООО__Товары_для_животных_.Interfaces;
using ООО__Товары_для_животных_.Models;

namespace ООО__Товары_для_животных_.Classes
{
    public class ListProducts : IDBListProducts
    {
        public List<Product> GetListProduct()
        {
            return DB.Instance.Products.ToList();
        }
    }
}
