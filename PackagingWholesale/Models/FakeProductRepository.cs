
using System.Collections.Generic;
using System.Linq;

namespace PackagingWholesale.Models
{
    public class FakeProductRepository /*: IProductRepository*/
    {
        public IQueryable<Product> Products => new List<Product>
        {
            new Product {Name = "Worek foliowy 300x400" , Price = 19.25m},
            new Product {Name = "Worek jutowy 650x1050" , Price = 5.50m},
            new Product {Name = "Worek raszlowy 300x500" , Price = 0.95m}
        }.AsQueryable<Product>();
    }
}
