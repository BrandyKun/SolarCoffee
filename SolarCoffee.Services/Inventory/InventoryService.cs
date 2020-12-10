using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDBContext _dbContext;
        private readonly ILogger<InventoryService> _logger;
        public InventoryService(SolarDBContext dbContext, ILogger<InventoryService> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        /// <summary>
        /// Creates a Snapshots record suing the provided ProductInventory instance
        /// </summary>
        /// <param name="inventory"></param>
        private void CreateSnapshot(ProductInvetory inventory)
        {
            var snapshots = new ProductInventorySnapshot
            {
                SnapshotTime = DateTime.UtcNow,
                Product = inventory.Product,
                QuantityHold = inventory.QuantityHold
            };

            _dbContext.Add(snapshots);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Gets a aProductInventory instance by Product Id
        /// </summary>
        /// <param name="GetByProductId"></param>
        /// <returns></returns>
        public ProductInvetory GetByProductId(int productId)
        {
            return _dbContext.ProductInvetories
                .Include(i => i.Product)
                .FirstOrDefault(i => i.Product.Id == productId);
        }

        /// <summary>
        /// Returns all current inventory from the database
        /// </summary>
        /// <returns></returns>
        public List<ProductInvetory> GetCurrentInventory()
        {
            return _dbContext.ProductInvetories
            .Include(pi => pi.Product)
            .Where(pi => !pi.Product.IsArchived)
            .ToList();
        }


        /// <summary>
        /// Return Snapshot history for the previous 6 hours
        /// </summary>
        /// <returns></returns>
        public List<ProductInventorySnapshot> GetSnapshotsHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);

            return _dbContext.ProductInventorySnapshots
                .Include(i => i.Product)
                .Where(i => i.SnapshotTime > earliest 
                                && !i.Product.IsArchived)
                .ToList();
        }


        /// <summary>
        /// Updates number of units available of the provided product id
        /// adjust QuanityOnHand by adjustment value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adjustment">number if units added / removed from inventory</param>
        /// <returns></returns>
        public ServiceResponse<ProductInvetory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = _dbContext.ProductInvetories
                    .Include(i => i.Product)
                    .FirstOrDefault(i => i.Product.Id == id);

                inventory.QuantityHold += adjustment;

                try
                {
                    CreateSnapshot(inventory);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error Creating inventory snapshots");
                    _logger.LogError(e.StackTrace);
                }

                _dbContext.SaveChanges();

                return new ServiceResponse<ProductInvetory>
                {
                    IsSuccessfull = true,
                    Data = inventory,
                    Message = $"Product {id} inventory adjusted",
                    Time = DateTime.UtcNow
                };
            }
            catch
            {
                return new ServiceResponse<ProductInvetory>
                {
                    IsSuccessfull = false,
                    Data = null,
                    Message = $"Error updating {id} inventory adjusted",
                    Time = DateTime.UtcNow
                };
            }
        }
    }
}