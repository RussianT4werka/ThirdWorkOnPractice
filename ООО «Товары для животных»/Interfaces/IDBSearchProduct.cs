using System.Collections.Generic;
using ООО__Товары_для_животных_.Models;

namespace ООО__Товары_для_животных_.Interfaces
{
    internal interface IDBSearchProduct
    {
        List<Product> GetAllIncludeListProduct(string SearchText);
    }
}