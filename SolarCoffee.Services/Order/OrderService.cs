using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDBContext _dbContext;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        public OrderService(SolarDBContext dbContext, ILogger<OrderService> logger, IProductService productService, IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
            _logger = logger;
            _dbContext = dbContext;
        }

        public ServiceResponse<bool> GenerateInvoiceForOrder(SalesOrder order)
        {

            _logger.LogInformation("Generating a new order");

            foreach (var item in order.SalesOrderItems)
            {
                item.Productd = _productService.GetProductByID(item.Productd.Id);
                item.Quantity = item.Quantity;
                var inventoryId = _inventoryService.GetByProductId(item.Productd.Id).Id;

                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            };

            try
            {
                _dbContext.SalesOrders.Add(order);
                _dbContext.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccessfull = true,
                    Data = true,
                    Message = "Open order created",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccessfull = false,
                    Data = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow
                };
            }
        }


        /// <summary>
        /// Get all SalesOrders in the system
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SalesOrder> GetOrders()
        {
            return _dbContext.SalesOrders
                .Include(s => s.Customer)
                    .ThenInclude(c => c.PrimaryAddress)
                .Include(s => s.SalesOrderItems)
                    .ThenInclude(item => item.Productd)
                .ToList();
        }

        /// <summary>
        /// Marks an open SalesOrder as paid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<bool> MarkFulfilled(int id)
        {
            var order = _dbContext.SalesOrders.Find(id);
            order.UpdatedOn = DateTime.UtcNow;
            order.IsPaid = true;

            try
            {
                _dbContext.SalesOrders.Add(order);
                _dbContext.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccessfull = true,
                    Data = true,
                    Message = $"Order {order.Id} closed: invoice paid in full.",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccessfull = false,
                    Data = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow
                };
            }
        }
    }
}