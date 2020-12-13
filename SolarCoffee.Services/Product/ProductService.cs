using System.Collections.Generic;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System.Linq;
using System;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDBContext _dbContext;
        public ProductService(SolarDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int id)
        {
            try
            {
                var product = _dbContext.Products.Find(id);
                product.IsArchived = true;
                _dbContext.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "Archived Product",
                    IsSuccessfull = true
                };
            }
            catch (Exception e) 
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = null,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSuccessfull = false
                };
            }
        }

        /// <summary>
        /// Adds a new product to the database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _dbContext.Products.Add(product);

                var newInventory = new ProductInvetory
                {
                    Product = product,
                    QuantityHold = 0,
                    IdealQuantity = 10
                };

                _dbContext.ProductInvetories.Add(newInventory);
                _dbContext.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = "New Product Saved",
                    IsSuccessfull = true
                };
            }
            catch(Exception e) 
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    Data = product,
                    Time = DateTime.UtcNow,
                    Message = e.StackTrace,
                    IsSuccessfull = false
                };
            }
        }

        /// <summary>
        /// Retrive all prodcut from database
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        /// <summary>
        /// /Retrive a product by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductByID(int id)
        {
            return _dbContext.Products.Find(id);
        }
    }
}