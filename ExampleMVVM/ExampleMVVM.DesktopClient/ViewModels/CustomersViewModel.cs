using ExampleMVVM.DesktopClient.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ExampleMVVM.DesktopClient.ViewModels
{
    public class CustomersViewModel : WorkspaceViewModel
    {
        private readonly CustomerRepository _customerRepository;

        public CustomersViewModel(CustomerRepository customerRepository)
        {

            _customerRepository = customerRepository ?? throw new ArgumentNullException("customerRepository");
            _customerRepository.CustomerAdded += OnCustomerAddedToRepository;
            DisplayName = "All Customers";
            CreateAllCustomers();
        }

        public ObservableCollection<CustomerViewModel> Customers { get; private set; }

        public decimal TotalSelectedSales =>
            Customers.Sum(customer => customer.IsSelected ? customer.TotalSales : 0M);

        private void OnCustomerAddedToRepository(object sender, CustomerAddedEventArgs e)
        {
            var customer = new CustomerViewModel(e.NewCustomer, _customerRepository);
            Customers.Add(customer);
        }

        private void CreateAllCustomers()
        {
            List<CustomerViewModel> customers =
                _customerRepository.GetCustomers().Select(
                    customer => new CustomerViewModel(customer, _customerRepository))
                .ToList();

            foreach (var customer in customers)
            {
                customer.PropertyChanged += OnCustomerViewModelPropertyChanged;
            }

            Customers = new ObservableCollection<CustomerViewModel>(customers);
            Customers.CollectionChanged += OnCustomersCollectionChanged;
        }

        private void OnCustomerViewModelPropertyChanged(object sender,
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomerViewModel.IsSelected))
            {
                OnPropertyChanged(nameof(TotalSelectedSales));
            }
        }

        private void OnCustomersCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (CustomerViewModel customer in e.NewItems)
                {
                    customer.PropertyChanged += OnCustomerViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (CustomerViewModel customer in e.OldItems)
                {
                    customer.PropertyChanged -= OnCustomerViewModelPropertyChanged;
                }
            }
        }
    }
}
