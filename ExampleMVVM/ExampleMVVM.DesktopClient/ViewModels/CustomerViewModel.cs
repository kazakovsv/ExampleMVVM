using ExampleMVVM.DesktopClient.Common;
using ExampleMVVM.DesktopClient.Models;
using ExampleMVVM.DesktopClient.Repositories;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ExampleMVVM.DesktopClient.ViewModels
{
    public class CustomerViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        private static readonly CustomerType[] _customerTypes =
        {
            CustomerType.None,
            CustomerType.Person,
            CustomerType.Company
        };

        private readonly Customer _customer;
        private readonly CustomerRepository _customerRepository;
        private CustomerType _customerType;
        private bool _isSelected;
        private DelegateCommand _saveCommand;

        public CustomerViewModel(Customer customer, CustomerRepository customerRepository)
        {
            _customer = customer ?? throw new ArgumentNullException(nameof(customer));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _customerType = CustomerType.None;
        }

        public string FirstName
        {
            get => _customer.FirstName;
            set
            {
                if (value == _customer.FirstName)
                {
                    return;
                }

                _customer.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _customer.LastName;
            set
            {
                if (value == _customer.LastName)
                {
                    return;
                }

                _customer.LastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _customer.Email;
            set
            {
                if (value == _customer.Email)
                {
                    return;
                }

                _customer.Email = value;
                OnPropertyChanged();
            }
        }

        public bool IsCompany => _customer.IsCompany;

        public decimal TotalSales => _customer.TotalSales;

        public CustomerType CustomerType
        {
            get => _customerType;
            set
            {
                if (value == _customerType)
                {
                    return;
                }

                _customerType = value;
                _customer.IsCompany = _customerType == CustomerType.Company;

                OnPropertyChanged();
                OnPropertyChanged(nameof(LastName));
            }
        }

        public CustomerType[] CustomerTypes => _customerTypes;

        public override string DisplayName
        {
            get
            {
                if (IsNewCustomer)
                {
                    return "New Customer";
                }
                else if (_customer.IsCompany)
                {
                    return _customer.FirstName;
                }
                else
                {
                    return $"{_customer.LastName} {_customer.FirstName}";
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected)
                {
                    return;
                }

                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(OnSave, CanSave));

        private bool CanSave()
        {
            return _customer.IsValid && string.IsNullOrEmpty(ValidateCustomerType());
        }

        private void OnSave()
        {
            if (!_customer.IsValid)
            {
                throw new InvalidOperationException("Cannot save an invalid customer.");
            }

            if (IsNewCustomer)
            {
                _customerRepository.AddCustomer(_customer);
            }

            OnPropertyChanged(nameof(DisplayName));
        }

        bool IsNewCustomer => !_customerRepository.ContainsCustomer(_customer);

        public string Error => (_customer as IDataErrorInfo).Error;

        public string this[string propertyName]
        {
            get
            {
                string error;

                if (propertyName == nameof(CustomerType))
                {
                    error = ValidateCustomerType();
                }
                else
                {
                    error = (_customer as IDataErrorInfo)[propertyName];
                }

                CommandManager.InvalidateRequerySuggested();

                return error;
            }
        }

        private string ValidateCustomerType()
        {
            if (CustomerType == CustomerType.Company ||
                CustomerType == CustomerType.Person)
            {
                return null;
            }

            return "Customer type must be selected";
        }
    }
}
