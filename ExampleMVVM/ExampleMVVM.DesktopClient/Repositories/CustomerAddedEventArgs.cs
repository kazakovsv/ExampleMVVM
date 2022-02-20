using ExampleMVVM.DesktopClient.Models;
using System;

namespace ExampleMVVM.DesktopClient.Repositories
{
    public class CustomerAddedEventArgs : EventArgs
    {
        public CustomerAddedEventArgs(Customer newCustomer)
        {
            NewCustomer = newCustomer;
        }

        public Customer NewCustomer { get; private set; }
    }
}