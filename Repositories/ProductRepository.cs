using Microsoft.EntityFrameworkCore;
using GameStore.Data;
using GameStore.Interfaces;
using GameStore.Models;

namespace GameStore.Repositories
{
    public class ProductRepository : IProduct
    {
        //================================ Предидущая реализация ========================================
        //private List<Product> products;
        //public ProductRepository()
        //{
        //    products = new List<Product>();
        //}

        //public void AddProduct(Product product)
        //{
        //    products.Add(product);
        //}

        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return products;
        //}
        //================================================================================================

        private ApplicationContext _context;
        public ProductRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.Include(e=>e.Category);
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Include(e=>e.Category).FirstOrDefault(e=>e.Id==id);
        }

        public void UpdateProduct(Product product)
        {
            Product product2 = GetProduct(product.Id);
            product2.Name = product.Name;
            product2.CategoryId = product.CategoryId;
            product2.RetailPrice = product.RetailPrice;
            product2.PurchasePrice = product.PurchasePrice;
            //_context.Products.Update(product); тут будут обновляться все поля, даже если мы не меняли их, а вверху мы меняем только те, которые нужно
            _context.SaveChanges();
        }
        public void UpdateAll(Product[] products)
        {
            Dictionary<int, Product> data = products.ToDictionary(e => e.Id);
            IEnumerable<Product> baseline = _context.Products.Where(e => data.Keys.Contains(e.Id));
            foreach (Product product in baseline)
            {
                Product requestProduct = data[product.Id];
                product.Name = requestProduct.Name;
                product.Category = requestProduct.Category;
                product.RetailPrice = requestProduct.RetailPrice;
                product.PurchasePrice = requestProduct.PurchasePrice;
            }
            //_context.Products.UpdateRange(products);
            _context.SaveChanges();
        }
        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
