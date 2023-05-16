using System.Collections.Generic;
using ООО__Товары_для_животных_.Models;

namespace ООО__Товары_для_животных_.Interfaces
{
    public interface IDBListManufacturers
    {
        List<Manufacturer> GetListManufacturers();
    }
}