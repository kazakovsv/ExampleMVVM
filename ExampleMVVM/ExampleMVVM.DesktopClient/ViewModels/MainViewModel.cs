using ExampleMVVM.DesktopClient.Common;
using ExampleMVVM.DesktopClient.Models;
using ExampleMVVM.DesktopClient.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ExampleMVVM.DesktopClient.ViewModels
{
    public class MainViewModel : WorkspaceViewModel
    {
        private readonly CustomerRepository _customerRepository;
        private ReadOnlyCollection<CommandViewModel> _commands;
        private ObservableCollection<WorkspaceViewModel> _workspaces;

        public MainViewModel(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));

            DisplayName = "ExampleMVVM";
        }

        public ReadOnlyCollection<CommandViewModel> Commands =>
            _commands ?? (_commands = new ReadOnlyCollection<CommandViewModel>(CreateCommands()));

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += OnWorkspacesCollectionChanged;
                }

                return _workspaces;
            }
        }

        private void OnWorkspacesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.RequestClose += OnWorkspaceRequestClose;
                }
            }

            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.RequestClose -= OnWorkspaceRequestClose;
                }
            }
        }

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            Workspaces.Remove(workspace);
        }

        private IList<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("View all customers",
                    new DelegateCommand(ShowAllCustomers)),

                new CommandViewModel("Create new customer",
                    new DelegateCommand(CreateNewCustomer))
            };
        }

        private void ShowAllCustomers()
        {
            CustomersViewModel workspace =
                Workspaces.FirstOrDefault(vm => vm is CustomersViewModel)
                as CustomersViewModel;

            if (workspace == null)
            {
                workspace = new CustomersViewModel(_customerRepository);
                Workspaces.Add(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        private void CreateNewCustomer()
        {
            Customer customer = Customer.Empty;
            CustomerViewModel workspace = new CustomerViewModel(customer, _customerRepository);
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);

            if (collectionView != null)
            {
                collectionView.MoveCurrentTo(workspace);
            }
        }
    }
}
