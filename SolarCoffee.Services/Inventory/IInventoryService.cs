using System.Collections.Generic;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public interface IInventoryService
    {
        public List<ProductInvetory> GetCurrentInventory();
        public ServiceResponse<ProductInvetory> UpdateUnitsAvailable(int id, int adjustment);
        public ProductInvetory GetByProductId(int productId);
        public List<ProductInventorySnapshot> GetSnapshotsHistory();

    }
}