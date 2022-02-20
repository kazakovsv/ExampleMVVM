using ExampleMVVM.DesktopClient.Models;
using System;
using System.Collections.Generic;

namespace ExampleMVVM.DesktopClient.Repositories
{
    public class CustomerRepository
    {
        private readonly List<Customer> _customers;

        public CustomerRepository()
        {
            _customers = LoadCutomers();
        }

        public event EventHandler<CustomerAddedEventArgs> CustomerAdded;

        private void OnCustomerAdded(CustomerAddedEventArgs e)
        {
            CustomerAdded?.Invoke(this, e);
        }

        public void AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            if (!_customers.Contains(customer))
            {
                _customers.Add(customer);
                OnCustomerAdded(new CustomerAddedEventArgs(customer));
            }
        }

        public List<Customer> GetCustomers()
        {
            return new List<Customer>(_customers);
        }

        private static List<Customer> LoadCutomers()
        {
            return new List<Customer>
            {
                Customer.Create("Алеша", "Попович", "alex@gmail.com", false, 1250M),
                Customer.Create("Илья", "Муромец", "ilya@gmail.com", false, 2450M),
                Customer.Create("Добрыня", "Никитич", "dobr@gmail.com", false, 3850M),
                Customer.Create("IsSoft", string.Empty, "issoft@gmail.com", true, 45000M),
                Customer.Create("Epam", string.Empty, "epam@gmail.com", true, 50000M),
            };
        }

        public bool ContainsCustomer(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            return _customers.Contains(customer);
        }
    }
}
