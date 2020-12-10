using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDBContext _dbContext;
        public CustomerService(SolarDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Customer> GetAllCustomer()
        {
            return _dbContext.Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();

            // return Ok("");
        }

        /// <summary>
        /// Adds New Customer Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Data.Models.Customer GetById(int id)
        {
            return _dbContext.Customers.FirstOrDefault(c => c.Id == id);
        }

        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _dbContext.Customers.Add(customer);
                _dbContext.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccessfull = true,
                    Message = "New Customer added",
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
            catch(Exception e) 
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccessfull = false,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
        }

        /// <summary>
        /// Deletes a customer record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ServiceResponse<bool></returns>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _dbContext.Customers.FirstOrDefault(c => c.Id == id);

            if(customer == null ) {
                return new ServiceResponse<bool>
                {
                    IsSuccessfull = false,
                    Message = "Customer to delete not found!",
                    Time = DateTime.UtcNow,
                    Data = false
                };
            }

            try 
            {
                _dbContext.Customers.Remove(customer);
                _dbContext.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccessfull = false,
                    Message = "Customer to delete not found!",
                    Time = DateTime.UtcNow,
                    Data = false
                };
            }

            catch (Exception e) 
            {
                return new ServiceResponse<bool>
                {
                    IsSuccessfull = true,
                    Message = e.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = true
                };
            }
        }

    }
}