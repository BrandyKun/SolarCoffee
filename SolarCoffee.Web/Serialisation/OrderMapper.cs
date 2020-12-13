using System;
using System.Collections.Generic;
using System.Linq;
using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Serialisation
{
    public class OrderMapper
    {
        public static SalesOrder SerialisationInvoiceToOrder(InvoiceModel invoice)
        {
            var salesOrderItems = invoice.LineItems
                .Select(item => new SalesOrderItem
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Productd = ProductMapper.SerializeProductModel(item.Product)
                }).ToList();

            return new SalesOrder
            {
                SalesOrderItems = salesOrderItems,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };
        }

        public static List<OrderModel> SerializeOrdersToViewModels(IEnumerable<SalesOrder> orders)
        {
            return orders.Select(o => new OrderModel
            {
                Id = o.Id,
                CreatedOn = o.CreatedOn,
                UpdatedOn = o.CreatedOn,
                SalesOrderItems = SerializeSalesOrderItems(o.SalesOrderItems),
                Customer = CustomerMapper.SerializeCustomer(o.Customer),
                IsPaid = o.IsPaid
            }).ToList();
        } 

        private static List<SalesOrderItemModel> SerializeSalesOrderItems(IEnumerable<SalesOrderItem> orderItems)
        {
            return orderItems.Select(item => new SalesOrderItemModel
            {
                Id = item.Id,
                Quantity = item.Quantity,
                Product = ProductMapper.SerializeProductModel(item.Productd)
            }).ToList();
        }
    }
}