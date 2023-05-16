using Microsoft.EntityFrameworkCore;
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
    internal class AllIncludeListProduct : IDBSearchProduct
    {
        public List<Product> GetAllIncludeListProduct(string SearchText)
        {
            return DB.Instance.Products
                                  .Include(p => p.ProductManufacturer)
                                  .Include(p => p.ProductProvider)
                                  .Include(p => p.OrderProducts)
                                  .Include(p => p.ProductCategory)
                                  .Where(s => s.ProductArticleNumber.Contains(SearchText) ||
                                              s.ProductCategory.Title.Contains(SearchText) ||
                                              s.ProductDescription.Contains(SearchText) ||
                                              s.ProductManufacturer.Title.Contains(SearchText) ||
                                              s.ProductTitle.Contains(SearchText) ||
                                              s.ProductProvider.Title.Contains(SearchText)).ToList();
        }
    }
}
