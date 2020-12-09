using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services
{
    public interface IProductService
    {
        List<Data.Models.Product> GetAllProducts();
        Data.Models.Product GetProductByID(int id);
        ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product);
        ServiceResponse<Data.Models.Product> ArchiveProduct(int id);
    }
}